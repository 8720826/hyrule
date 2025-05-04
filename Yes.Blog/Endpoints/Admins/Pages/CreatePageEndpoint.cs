namespace Yes.Blog.Endpoints.Admins.Pages
{
    public class CreatePageEndpoint : PageEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapPost("/pages", Handle).WithRequestValidation<Request>();

		public record Request(
			string Title,
			string? Slug,
			string Content,
			string? CoverUrl,
			string? Summary,
			int Sort
		);

		public class RequestValidator : AbstractValidator<Request>
		{
			public RequestValidator()
			{
				RuleFor(x => x.Content).NotEmpty().WithMessage("文章内容不能为空！").NotNull().WithMessage("文章内容不能为空！");

				RuleFor(x => x.Title).NotEmpty().WithMessage("标题不能为空！").NotNull().WithMessage("标题不能为空！");

				RuleFor(x => x.CoverUrl).MaximumLength(256).WithMessage("封面图地址长度不能超过256！");


				RuleFor(x => x.Slug).MaximumLength(128).WithMessage("缩略名长度不能超过128！");

				RuleFor(x => x.Slug).NotEmpty().WithMessage("缩略名不能为空！")
					.Must(x => x.IsSlug())
					.WithMessage(x => $"缩略名{x.Slug}无效，只能为字母数字横线和下划线组合，且不能使用纯数字！");
			}


		}

		private async Task<IResult> Handle([FromBody] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var command = mapper.Map<CreatePageCommand>(request);
			var response = await mediator.Send(command, cancellationToken);


			return Result.Ok(response);
		}
	}

	internal class CreatePageMappingProfile : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.ForType<CreatePageEndpoint.Request, CreatePageCommand>();
		}
	}
}
