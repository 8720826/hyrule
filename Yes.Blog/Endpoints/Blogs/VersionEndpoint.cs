namespace Yes.Blog.Endpoints.Blogs
{

    public class VersionEndpoint : BlogEndpointScheme, IEndpoint
    {
        public void Map(IEndpointRouteBuilder app) => app.MapGet("/version", Handle);

        private string Handle(
          CancellationToken cancellationToken)
        {
            return BlogHelper.GetVersion();
        }
    }
}
