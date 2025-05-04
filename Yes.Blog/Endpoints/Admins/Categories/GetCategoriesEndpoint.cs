namespace Yes.Blog.Endpoints.Admins.Categories
{
    public class GetCategoriesEndpoint : CategoryEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapGet("/categories", Handle);

		public record Request();


		private async Task<IResult> Handle([AsParameters] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var query = new GetCategoriesQuery();
			var response = await mediator.Send(query, cancellationToken);


			return Result.Ok(response);
		}
	}

}
