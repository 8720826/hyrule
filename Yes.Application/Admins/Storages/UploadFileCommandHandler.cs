namespace Yes.Application.Admins.Storages
{
    public record UploadFileCommand(IFormFile File) : IRequest<UploadFileCommandResponse>;

    public record UploadFileCommandResponse(string Key);
    public class UploadFileCommandHandler(IWebHostEnvironment env,
        IOptionsMonitor<BlogSettings> options) : IRequestHandler<UploadFileCommand, UploadFileCommandResponse>
    {
        private readonly IWebHostEnvironment _env = env;
        private readonly BlogSettings _settings = options.CurrentValue;

        public async Task<UploadFileCommandResponse> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
          
            try
            {
                var file = request.File;

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                var allowedExts = _settings.AllowedUploadExts;
                if (!allowedExts.Contains(ext))
                {
                    throw new FileTypeException(string.Join(",", allowedExts));
                }

                if (file.Length > _settings.UploadSizeLimit)
                {
                    throw new FileOverflowException($"{_settings.UploadSizeLimit/1024/1024}M");
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

                var url = $"{baseUploadPath}/{datePath}/{newFileName}";

                return new UploadFileCommandResponse(url);
            }
            catch (Exception ex) when (ex is OverflowException || ex is InvalidDataException)
            {
                throw new FileOverflowException();
            }

        }
    }
}
