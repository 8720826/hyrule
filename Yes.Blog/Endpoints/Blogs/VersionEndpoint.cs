namespace Yes.Blog.Endpoints.Blogs
{

    public class VersionEndpoint : BlogEndpointScheme, IEndpoint
    {
        public void Map(IEndpointRouteBuilder app) => app.MapGet("/version", Handle).AllowAnonymous();

        private string Handle(
          CancellationToken cancellationToken)
        {
            return BlogHelper.GetVersion();
        }
    }
}
