namespace Yes.Blog.Endpoints.Admins.Articles
{
    public class GetArticleEndpoint : ArticleEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapGet("/articles/{id}", Handle);

		public record Request([FromRoute(Name = "id")] int Id);


		private async Task<IResult> Handle(
			[AsParameters] Request request,
			IMediator mediator,
			IMapper mapper,
			CancellationToken cancellationToken)
		{
			var query = new GetArticleQuery(request.Id);
			var response = await mediator.Send(query, cancellationToken);

			return Result.Ok(response);
		}
	}
}
