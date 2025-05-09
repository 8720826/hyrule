namespace Yes.Blog.Endpoints.Admins.Themes
{
    public class GetThemeFileEndpoint : ThemeEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapGet("/themes/{themeName}/files/{fileName}", Handle);

        public record Request([FromRoute(Name = "themeName")] string ThemeName, [FromRoute(Name = "fileName")] string FileName);


        private async Task<IResult> Handle([AsParameters] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var query = new GetThemeFileQuery(request.ThemeName, request.FileName);
			var response = await mediator.Send(query, cancellationToken);


			return Result.Ok(response);
		}
	}

}
