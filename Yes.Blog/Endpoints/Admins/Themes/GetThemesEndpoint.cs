namespace Yes.Blog.Endpoints.Admins.Themes
{
    public class GetThemesEndpoint : ThemeEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapGet("/themes", Handle);

		public record Request();


		private async Task<IResult> Handle([AsParameters] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var query = new GetThemesQuery();
			var response = await mediator.Send(query, cancellationToken);


			return Result.Ok(response);
		}
	}

}
