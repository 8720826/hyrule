namespace Yes.Blog.Endpoints.Admins.Users
{
    public class DeleteUserEndpoint : UserEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapDelete("/users", Handle);

		public record Request(int Id);

		private async Task<IResult> Handle([AsParameters] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var command = mapper.Map<DeleteUserCommand>(request);
			var response = await mediator.Send(command, cancellationToken);


			return Result.Ok(response);
		}
	}


}
