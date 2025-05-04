namespace Yes.Blog.Endpoints.Admins.Themes
{
    public class UpdateThemeEndpoint : ThemeEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapPut("/themes", Handle).WithRequestValidation<Request>();

        [Scalar.AspNetCore.ExcludeFromApiReference]
        internal record Request(
			string Theme
		);

        internal class RequestValidator : AbstractValidator<Request>
		{
			public RequestValidator()
			{
				RuleFor(x => x.Theme).NotEmpty().WithMessage("请选择主题！");

            }
		}

		private async Task<IResult> Handle([FromBody] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var command = mapper.Map<UpdateThemeCommand>(request);
			var response = await mediator.Send(command, cancellationToken);


			return Result.Ok(response);
		}
	}

	internal class UpdateThemeMappingProfile : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.ForType<UpdateThemeEndpoint.Request, UpdateThemeCommand>();
		}
	}
}
