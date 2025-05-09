
namespace Yes.Blog.Endpoints.Admins.Themes
{
    public class UpdateThemeFileEndpoint : ThemeEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapPut("/themes/{themeName}/files/{fileName}", Handle);//.WithRequestValidation<Request>();

        internal record Request(
			string Content 
        );

        internal class RequestValidator : AbstractValidator<Request>
		{
			public RequestValidator()
			{
                //RuleFor(x => x.ThemeName).NotEmpty().WithMessage("请选择主题！");
                //RuleFor(x => x.FileName).NotEmpty().WithMessage("请选择文件！");
                RuleFor(x => x.Content).NotEmpty().WithMessage("请输入模板内容！");
            }
		}

		private async Task<IResult> Handle([FromBody] Request request, 
			[FromRoute(Name = "themeName")] string themeName,
            [FromRoute(Name = "fileName")] string fileName,
            IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var command = mapper.Map<UpdateThemeFileCommand>(request);
            command = command with { ThemeName = themeName, FileName = fileName };
            var response = await mediator.Send(command, cancellationToken);


			return Result.Ok(response);
		}
	}

	internal class UpdateThemeFileMappingProfile : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.ForType<UpdateThemeFileEndpoint.Request, UpdateThemeFileCommand>();
		}
	}
}
