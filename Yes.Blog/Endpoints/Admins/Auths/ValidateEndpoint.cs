using Yes.Infrastructure.Authorizations;

namespace Yes.Blog.Endpoints.Admins.Auths
{
    public class ValidateEndpoint : AuthEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapPost("/validate", Handle).AllowAnonymous();


		private async Task<IResult> Handle([FromHeader] string authorization, 
            IMediator mediator, 
            IMapper mapper, 
            HttpContext context,
            IOptionsMonitor<BlogSettings> options,CancellationToken cancellationToken)
		{
            var token = authorization?.Replace("Bearer ", "");

            try
            {
                var command = new ValidateCommand(token);
                var response = await mediator.Send(command, cancellationToken);

                await context.SignIn(response.Identity, response.TokenLifetimeMinutes);


                return Result.Ok(new { IsValid = true });
            }
            catch (Exception ex)
            {
                return Result.Ok(new { IsValid = false, Error = ex.Message });
            }
		}
	}
}
