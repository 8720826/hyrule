namespace Yes.Blog.Endpoints.Admins.Storages
{
    public class StorageEndpointScheme
    {
        public int Priority { get; set; } = 3;
        public string Prefix { get; set; } = "admin/api";

        public string[] Tags { get; set; } = new[] { "Storages" };
    }
}
