namespace Yes.Application.Admins.Storages
{
    public record GenerateQiniuTokenCommand(StorageTypeEnum StorageType) : IRequest<GenerateQiniuTokenCommandResponse>;

    public record GenerateQiniuTokenCommandResponse(string StorageType, string Token, string Prefix);
    public class GenerateQiniuTokenCommandHandler(BlogDbContext db, IOptionsMonitor<BlogSettings> options) : IRequestHandler<GenerateQiniuTokenCommand, GenerateQiniuTokenCommandResponse>
    {
        private readonly BlogDbContext _db = db;
        private readonly BlogSettings _settings = options.CurrentValue;
        public async Task<GenerateQiniuTokenCommandResponse> Handle(GenerateQiniuTokenCommand request, CancellationToken cancellationToken)
        {
            if (_settings.Storage.StorageType != StorageTypeEnum.Qiniu.ToString())
            {
                throw new StorageTypeException(StorageTypeEnum.Qiniu.ToString());
            }

            if (_settings.Storage.Qiniu == null)
            {
                throw new StorageTypeException(StorageTypeEnum.Qiniu.ToString());
            }

            if (string.IsNullOrEmpty(_settings.Storage.Qiniu.AccessKey) ||
                string.IsNullOrEmpty(_settings.Storage.Qiniu.SecretKey) ||
                string.IsNullOrEmpty(_settings.Storage.Qiniu.Bucket) ||
                string.IsNullOrEmpty(_settings.Storage.Qiniu.Domain)
                )
            {
                throw new StorageTypeException(StorageTypeEnum.Qiniu.ToString());
            }

            var token = QiniuProvider.CreateToken(_settings.Storage.Qiniu.AccessKey, _settings.Storage.Qiniu.SecretKey, _settings.Storage.Qiniu.Bucket);

            var prefix = $"{_settings.Storage.Qiniu.Domain}/";

            return new GenerateQiniuTokenCommandResponse(request.StorageType.ToString(), token, prefix);
        }
    }
}
