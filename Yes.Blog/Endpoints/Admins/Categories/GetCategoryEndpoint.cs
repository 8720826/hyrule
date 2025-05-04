namespace Yes.Blog.Endpoints.Admins.Categories
{
    public class GetCategoryEndpoint : CategoryEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapGet("/categories/{id}", Handle);

		public record Request([FromRoute(Name = "id")] int Id);


		private async Task<IResult> Handle([AsParameters] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var query = new GetCategoryQuery(request.Id);
			var response = await mediator.Send(query, cancellationToken);


			return Result.Ok(response);
		}
	}

}
