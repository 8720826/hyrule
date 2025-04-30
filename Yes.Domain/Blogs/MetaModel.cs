namespace Yes.Domain.Blogs
{
    public class MetaModel
    {        
        /// <summary>
        /// 网页标题
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 网页关键词
        /// </summary>
        public string Keywords { get; set; } = string.Empty;

        /// <summary>
        /// 网页描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
