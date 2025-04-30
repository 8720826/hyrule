namespace Yes.Domain.Blogs
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string CoverUrl { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
