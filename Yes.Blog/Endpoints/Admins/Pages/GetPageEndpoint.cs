namespace Yes.Blog.Endpoints.Admins.Pages
{
    public class GetPageEndpoint : PageEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapGet("/pages/{id}", Handle);

		public record Request([FromRoute(Name = "id")] int Id);



		private async Task<IResult> Handle(
			[AsParameters] Request request,
			IMediator mediator,
			IMapper mapper,
			CancellationToken cancellationToken)
		{
			var query = new GetPageQuery(request.Id);
			var response = await mediator.Send(query, cancellationToken);

			return Result.Ok(response);
		}
	}
}
