namespace Yes.Blog.Endpoints.Admins.Users
{
    public class GetUsersEndpoint : AdminEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapGet("/users", Handle);

		public record Request();


		private async Task<IResult> Handle([AsParameters] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var query = new GetUsersQuery();
			var response = await mediator.Send(query, cancellationToken);


			return Result.Ok(response);
		}
	}

}
