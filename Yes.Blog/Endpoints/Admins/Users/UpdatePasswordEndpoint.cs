namespace Yes.Blog.Endpoints.Admins.Users
{
    public class UpdatePasswordEndpoint : AdminEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapPut("/users/Password", Handle).WithRequestValidation<Request>();

        public record Request(
			string OriginalPassword,
			string NewPassword,
			string ConfirmPassword
		);

		public class RequestValidator : AbstractValidator<Request>
		{
			public RequestValidator()
            {
                RuleFor(x => x.OriginalPassword).NotEmpty().WithMessage("请输入原密码！");
                RuleFor(x => x.NewPassword).NotEmpty().WithMessage("请输入新密码！");
                RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("请重复输入新密码！");

                RuleFor(x => x.NewPassword).Must((x, y) => x.ConfirmPassword == y).WithMessage("重复密码必须跟新密码一样");
            }

        }

		private async Task<IResult> Handle([FromBody] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var command = mapper.Map<UpdatePasswordCommand>(request);
			var response = await mediator.Send(command, cancellationToken);


			return Result.Ok(response);
		}
	}

	internal class UpdatePasswordMappingPassword : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.ForType<UpdatePasswordEndpoint.Request, UpdatePasswordCommand>();
		}
	}
}
