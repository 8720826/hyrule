namespace Yes.Domain.Articles
{
    /// <summary>
    /// 文章标签
    /// </summary>
    [Table("ArticleTag")]
    public class ArticleTagEntity
    {
        [Key]
        public int Id { get; set; }


        /// <summary>
        /// 文章Id
        /// </summary>
        public int ArticleId { get; set; }

        /// <summary>
        /// 标签Id
        /// </summary>
        public int TagId { get; set; }
    }
}