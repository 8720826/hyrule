namespace Yes.Domain.Configurations
{
    public class BlogSettings
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string DatabaseType { get; set; }

        public string DatabaseVersion { get; set; }

        public string Name { get; set; } = "我的博客";

        public string Logo { get; set; } = "https://img.yescent.com/FosQ9ua_nPhlNQwYgJwUXi9MFsB5.jpg";

        public string Url { get; set; } = "https://yescent.com";

        public int PageSizeOfHomepage { get; set; } = 5;

        public int PageSizeOfListpage { get; set; } = 5;

        public string ArticleRoute { get; set; } = "/post/{p_slug}.html";

        public string SearchRoute { get; set; } = "/s/{keyword}";

        public string PageRoute { get; set; } = "/page/{p_id}";

        public string CategoryRoute { get; set; } = "/cate/{c_id}";

        public string TagRoute { get; set; } = "/tag/{t_id}";

        public string ArchiveRoute { get; set; } = "/{year}/{month}";

        public string Keywords { get; set; } = "Hyrule,Blog,博客";

        public string Description { get; set; } = "Hyrule 是一个简洁的开源博客系统";

        public string BeiAn { get; set; } = "";

        public string Script { get; set; } = "";

        public string Theme { get; set; } = "default";

        public string[] AllowedUploadExts { get; set; } = [".gif", ".jpg", ".jpeg", ".png"];

        public int UploadSizeLimit { get; set; } = 1024 * 1024 * 20;

        public StorageSettings Storage { get; set; } = new StorageSettings()
        {
            StorageType = StorageTypeEnum.Local.ToString(),
            Qiniu = new QiniuSettings
            {

            }
        };
    }


}
