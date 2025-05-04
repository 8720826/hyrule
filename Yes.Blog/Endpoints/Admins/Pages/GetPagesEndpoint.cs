namespace Yes.Blog.Endpoints.Admins.Pages
{
    public class GetPagesEndpoint : PageEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapGet("/pages", Handle);

		public record Request();


		private async Task<IResult> Handle(
			[AsParameters] Request request,
			IMediator mediator,
			IMapper mapper,
			CancellationToken cancellationToken)
		{
			var query = new GetPagesQuery();
			var response = await mediator.Send(query, cancellationToken);

			return Result.Ok(response);
		}
	}
}
