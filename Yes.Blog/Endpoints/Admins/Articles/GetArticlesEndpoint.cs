namespace Yes.Blog.Endpoints.Admins.Articles
{
    public class GetArticlesEndpoint : ArticleEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapGet("/articles", Handle);

		public record Request(int? PageIndex);


		private async Task<IResult> Handle(
			[AsParameters] Request request,
			IMediator mediator,
			IMapper mapper,
			CancellationToken cancellationToken)
		{
			var query = new GetArticlesQuery(request.PageIndex, 15);
			var response = await mediator.Send(query, cancellationToken);

			return Result.Ok(response);
		}
	}
}
