namespace Yes.Blog.Endpoints.Admins.Comments
{
    public class CommentEndpointScheme
    {
        public int Priority { get; set; } = 3;
        public string Prefix { get; set; } = "admin/api";

        public string[] Tags { get; set; } = new[] { "Comments" };
    }
}
