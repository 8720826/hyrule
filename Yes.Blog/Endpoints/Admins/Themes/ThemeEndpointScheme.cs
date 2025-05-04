namespace Yes.Blog.Endpoints.Admins.Themes
{
    public class ThemeEndpointScheme
    {
        public int Priority { get; set; } = 3;
        public string Prefix { get; set; } = "admin/api";

        public string[] Tags { get; set; } = new[] { "Themes" };
    }
}
