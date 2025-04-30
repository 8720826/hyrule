
namespace Yes.Blog.Endpoints.Blogs
{
    public class SearchEndpoint : BlogEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapPost("/search", Handle).DisableAntiforgery();

        
        public record Request(
            string S
        );
        private async Task<Microsoft.AspNetCore.Http.IResult> Handle(
			[FromForm] Request request,
			IOptionsMonitor<BlogSettings> options,
			CancellationToken cancellationToken)
		{
			var settings = options.CurrentValue;
			if (string.IsNullOrEmpty(settings.ConnectionString) || string.IsNullOrEmpty(settings.DatabaseName) || string.IsNullOrEmpty(settings.DatabaseType))
			{
				return Results.Redirect("/Install/Index", false);
			}

			if (string.IsNullOrEmpty(request.S))
			{
                return Results.Redirect("/", false);
            }

			var url = settings.SearchRoute.Replace(BlogRouteConst.Keyword.ToPlaceholder(), HttpUtility.UrlEncode(request.S));

            return Results.Redirect(url, false);
		}
	}
}
