namespace Yes.Blog.Endpoints.Admins.Statistics
{
    public class StatEndpointScheme
    {
        public int Priority { get; set; } = 3;
        public string Prefix { get; set; } = "admin/api";

        public string[] Tags { get; set; } = new[] { "Statistics" };
    }
}
