using Yes.Infrastructure.Authorizations;

namespace Yes.Blog.Endpoints.Admins.Auths
{
    public class UserLoginEndpoint : AuthEndpointScheme, IEndpoint
	{

		public void Map(IEndpointRouteBuilder app) => app.MapPost("/login", Handle).WithRequestValidation<Request>().AllowAnonymous();

        public record Request(string Name, string Password);

		public class RequestValidator : AbstractValidator<Request>
		{
			public RequestValidator()
			{
				RuleFor(x => x.Name).NotEmpty().WithMessage("用户名不能为空！");
				RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空！");
			}
		}

		private async Task<IResult> Handle([FromBody] Request request, IMediator mediator, IMapper mapper, HttpContext context, CancellationToken cancellationToken)
		{
			var command = mapper.Map<UserLoginCommand>(request);
			var response = await mediator.Send(command, cancellationToken);

			await context.SignIn(response.Identity, response.TokenLifetimeMinutes);

			return Result.Ok(response);
		}
	}
}
