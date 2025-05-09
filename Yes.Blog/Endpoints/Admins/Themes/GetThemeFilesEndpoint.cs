namespace Yes.Blog.Endpoints.Admins.Themes
{
    public class GetThemeFilesEndpoint : ThemeEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapGet("/themes/{themeName}/files", Handle);

        public record Request([FromRoute(Name = "themeName")] string ThemeName);


        private async Task<IResult> Handle([AsParameters] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var query = new GetThemeFilesQuery(request.ThemeName);
			var response = await mediator.Send(query, cancellationToken);


			return Result.Ok(response);
		}
	}

}
