namespace Yes.Blog.Endpoints.Admins.Users
{
    public class UserEndpointScheme
    {
        public int Priority { get; set; } = 3;
        public string Prefix { get; set; } = "admin/api";

        public string[] Tags { get; set; } = new[] { "Users" };
    }
}
