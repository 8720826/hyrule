namespace Yes.Blog.Endpoints.Admins.Users
{
    public class UpdateProfileEndpoint : AdminEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapPut("/users/profile", Handle).WithRequestValidation<Request>();

        public record Request(
			string Email,
			string NickName,
			string Url
		);

		public class RequestValidator : AbstractValidator<Request>
		{
			public RequestValidator()
            {

                RuleFor(x => x.Email).NotEmpty().WithMessage("请输入邮箱！");
                RuleFor(x => x.NickName).NotEmpty().WithMessage("请输入昵称！");

            }
        }

		private async Task<IResult> Handle([FromBody] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var command = mapper.Map<UpdateProfileCommand>(request);
			var response = await mediator.Send(command, cancellationToken);


			return Result.Ok(response);
		}
	}

	internal class UpdateProfileMappingProfile : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.ForType<UpdateProfileEndpoint.Request, UpdateProfileCommand>();
		}
	}
}
