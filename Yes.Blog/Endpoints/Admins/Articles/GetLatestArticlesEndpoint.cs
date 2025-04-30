

namespace Yes.Blog.Endpoints.Admins.Articles
{
    public class GetLatestArticlesEndpoint : AdminEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapGet("/articles/Latest", Handle);



		private async Task<IResult> Handle(
			IMediator mediator,
			IMapper mapper,
			CancellationToken cancellationToken)
		{
			var query = new GetLatestArticlesQuery();
			var response = await mediator.Send(query, cancellationToken);

			return Result.Ok(response);
		}
	}
}
