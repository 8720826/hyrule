namespace Yes.Blog.Endpoints.Admins.Themes
{
    public class GetThemeFileEndpoint : ThemeEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapGet("/themes/{themeName}/file", Handle);



        private async Task<IResult> Handle(
			[FromRoute(Name = "themeName")] string themeName, 
			[FromQuery] string fileName, IMediator mediator, 
			IMapper mapper, CancellationToken cancellationToken)
		{
			var query = new GetThemeFileQuery(themeName, fileName);
			var response = await mediator.Send(query, cancellationToken);


			return Result.Ok(response);
		}
	}

}
