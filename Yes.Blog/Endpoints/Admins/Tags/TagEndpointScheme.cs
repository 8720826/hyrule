namespace Yes.Blog.Endpoints.Admins.Tags
{
    public class TagEndpointScheme
    {
        public int Priority { get; set; } = 3;
        public string Prefix { get; set; } = "admin/api";

        public string[] Tags { get; set; } = new[] { "Tags" };
    }
}
