namespace Yes.Blog.Endpoints.Admins.Users
{
    public class UpdateUserEndpoint : AdminEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapPut("/users", Handle).WithRequestValidation<Request>();

        public record Request(
			int Id,
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
            }
		}

		private async Task<IResult> Handle([FromBody] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var command = mapper.Map<UpdateUserCommand>(request);
			var response = await mediator.Send(command, cancellationToken);


			return Result.Ok(response);
		}
	}

	internal class UpdateUserMappingProfile : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.ForType<UpdateUserEndpoint.Request, UpdateUserCommand>();
		}
	}
}
