namespace Yes.Domain.Articles
{
    /// <summary>
    /// 标签
    /// </summary>
    [Table("Tag")]
    public class TagEntity
    {
        [Key]
        public int Id { get; set; }


        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 缩略名
        /// </summary>
        public string Slug { get; set; }


        public static TagEntity Create(string name, string slug="")
        {
            return new TagEntity()
            {
                Name = name ?? "",
                Slug = slug ?? name ?? "",
                CreateDate = DateTime.Now,
            };
        }

        public void Update(string slug)
        {
            Slug = slug ?? "";
        }
    }
}