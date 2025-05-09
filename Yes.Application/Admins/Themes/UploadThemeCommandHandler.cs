

namespace Yes.Application.Admins.Themes
{
    public record UploadThemeCommand(IFormFile File) : IRequest<UploadThemeCommandResponse>;

    public record UploadThemeCommandResponse(string DirName);
    public class UploadThemeCommandHandler(IWebHostEnvironment env,
        IOptionsMonitor<BlogSettings> options) : IRequestHandler<UploadThemeCommand, UploadThemeCommandResponse>
    {
        private readonly IWebHostEnvironment _env = env;
        private readonly BlogSettings _settings = options.CurrentValue;

        public async Task<UploadThemeCommandResponse> Handle(UploadThemeCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var file = request.File;

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                var allowedExts = _settings.AllowedUploadExts;
                if (!ext.Equals(".zip"))
                {
                    throw new FileTypeException(".zip");
                }

                if (file.Length > _settings.UploadSizeLimit)
                {
                    throw new FileOverflowException($"{_settings.UploadSizeLimit / 1024 / 1024}M");
                }


                var baseUploadPath = "uploads";
                var datePath = DateTime.Now.ToString("yyyyMM");


                var uploads = Path.Combine(_env.ContentRootPath, "files", baseUploadPath, datePath);
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                var hashKey = "";


                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    using (var cryptoProvider = SHA256.Create())
                    {
                        byte[] hashBytes = cryptoProvider.ComputeHash(memoryStream);
                        hashKey = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                    }
                }

                var newFileName = $"{hashKey}{ext}";

                var filePath = Path.Combine(uploads, newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var newDirName = Path.GetFileName(file.FileName).Replace(".zip", "");

                var targetDirectory = Path.Combine(_env.ContentRootPath, "files", "themes");

                ZipHelper.ExtractToDirectory(filePath, targetDirectory, newDirName);


                string targetFilePath = Path.Combine(targetDirectory, newDirName, "config.json");
                if (!File.Exists(targetFilePath))
                {
                    throw new InvalidDataException("缺少config.json文件");
                }

                return new UploadThemeCommandResponse(newDirName);
            }
            catch (Exception ex) when (ex is OverflowException)
            {
                throw new FileOverflowException();
            }
            catch (Exception ex) when (ex is InvalidDataException)
            {
                throw new FileNotSupportedException(ex.Message);
            }
        }
    }
}
