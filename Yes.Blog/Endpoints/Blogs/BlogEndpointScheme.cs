namespace Yes.Blog.Endpoints.Blogs
{
	public class BlogEndpointScheme
	{
		public int Priority { get; set; } = 0;

		public string Prefix { get; set; } = "";


        public string[] Tags { get; set; } = new[] { "Blog" };
    }
}
