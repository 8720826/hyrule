namespace Yes.Domain.Blogs
{
    public class TagModel
    {
        public int ArticleId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public string Slug { get; set; }

        public string Link { get; set; }
    }
}
