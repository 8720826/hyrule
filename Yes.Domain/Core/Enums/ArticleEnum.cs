namespace Yes.Domain.Core.Enums
{
    public enum ArticleTypeEnum
    {
        /// <summary>
        /// 文章
        /// </summary>
        Article = 1,

        /// <summary>
        /// 单页
        /// </summary>
        Page = 2
    }

    public enum ArticleStatusEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 已删除
        /// </summary>
        Deleted = 1,
        /// <summary>
        /// 草稿
        /// </summary>
        Draft = 2,
    }
}
