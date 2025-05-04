namespace Yes.Blog.Endpoints.Admins.Users
{
    public class CreateUserEndpoint : UserEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapPost("/users", Handle).WithRequestValidation<Request>();

        public record Request(
			string Name,
			string Email,
			string NickName,
			string Password
		);

		public class RequestValidator : AbstractValidator<Request>
		{
			public RequestValidator()
			{
                RuleFor(x => x.Name).NotEmpty().WithMessage("请输入用户名！");
                RuleFor(x => x.Email).NotEmpty().WithMessage("请输入邮箱！");
                RuleFor(x => x.NickName).NotEmpty().WithMessage("请输入昵称！");
                RuleFor(x => x.Password).NotEmpty().WithMessage("请输入用户密码！");
            }
		}

		private async Task<IResult> Handle([FromBody] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var command = mapper.Map<CreateUserCommand>(request);
			var response = await mediator.Send(command, cancellationToken);


			return Result.Ok(response);
		}
	}

	internal class CreateUserMappingProfile : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.ForType<CreateUserEndpoint.Request, CreateUserCommand>();
		}
	}
}
