namespace Yes.Blog.Endpoints.Admins.Auths
{
    public class UserLogoutEndpoint : AuthEndpointScheme, IEndpoint
	{

		public void Map(IEndpointRouteBuilder app) => app.MapPost("/logout", Handle).AllowAnonymous();


		private async Task<IResult> Handle(IMediator mediator, IMapper mapper, HttpContext context, CancellationToken cancellationToken)
		{
			var command = new UserLogoutCommand();
			var response = await mediator.Send(command, cancellationToken);

			await context.SignOut();

			return Result.Ok(response);
		}
	}
}
