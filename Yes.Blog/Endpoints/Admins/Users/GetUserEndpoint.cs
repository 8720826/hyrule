namespace Yes.Blog.Endpoints.Admins.Users
{
    public class GetUserEndpoint : AdminEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapGet("/users/{id}", Handle);

		public record Request([FromRoute(Name = "id")] int Id);


		private async Task<IResult> Handle([AsParameters] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var query = new GetUserQuery(request.Id);
			var response = await mediator.Send(query, cancellationToken);


			return Result.Ok(response);
		}
	}

}
