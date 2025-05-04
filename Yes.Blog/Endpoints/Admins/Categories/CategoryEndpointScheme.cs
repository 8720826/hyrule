namespace Yes.Blog.Endpoints.Admins.Categories
{
    public class CategoryEndpointScheme
    {
        public int Priority { get; set; } = 3;
        public string Prefix { get; set; } = "admin/api";

        public string[] Tags { get; set; } = new[] { "Categories" };
    }
}
