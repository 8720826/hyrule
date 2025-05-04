namespace Yes.Blog.Endpoints.Admins.Pages
{
    public class PageEndpointScheme
    {
        public int Priority { get; set; } = 3;
        public string Prefix { get; set; } = "admin/api";

        public string[] Tags { get; set; } = new[] { "Pages" };
    }
}
