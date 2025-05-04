namespace Yes.Blog.Endpoints.Admins.Articles
{
    public class GetLatestArticlesEndpoint : ArticleEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapGet("/articles/latest", Handle);



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
