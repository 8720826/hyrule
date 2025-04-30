namespace Yes.Domain.Configurations
{
	public class StorageSettings
    {
      

        public string StorageType { get; set; }

        public QiniuSettings Qiniu { get; set; }
    }

    public class QiniuSettings
    {
        [Display(Name = "AccessKey")]
        public string? AccessKey { get; set; } = string.Empty;

        [Display(Name = "SecretKey")]
        public string? SecretKey { get; set; } = string.Empty;


        [Display(Name = "域名")]
        public string? Domain { get; set; } = string.Empty;


        [Display(Name = "存储桶名称")]
        public string? Bucket { get; set; } = string.Empty;
    }
}
