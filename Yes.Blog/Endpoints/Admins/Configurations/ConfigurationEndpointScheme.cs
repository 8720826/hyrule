namespace Yes.Blog.Endpoints.Admins.Configurations
{
    public class ConfigurationEndpointScheme
    {
        public int Priority { get; set; } = 3;
        public string Prefix { get; set; } = "admin/api";

        public string[] Tags { get; set; } = new[] { "Configurations" };
    }
}
