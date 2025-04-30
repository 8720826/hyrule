namespace Yes.Blog.Endpoints.Admins.Articles
{
    public class CreateArticleEndpoint : AdminEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapPost("/articles", Handle).WithRequestValidation<Request>();

		public record Request(
			int CategoryId,
			string Title,
			string? Slug,
			string Content,
			string? CoverUrl,
			string? Summary,
			List<string> Tags,
			bool IsDraft
		);

		public class RequestValidator : AbstractValidator<Request>
		{
			public RequestValidator()
			{
				RuleFor(x => x.Content).NotEmpty().WithMessage("文章内容不能为空！").NotNull().WithMessage("文章内容不能为空！");

				RuleFor(x => x.Title).NotEmpty().WithMessage("标题不能为空！").NotNull().WithMessage("标题不能为空！").When(x => !x.IsDraft);

				RuleFor(x => x.CategoryId).NotEqual(0).WithMessage("请选择分类！").When(x => !x.IsDraft);

				RuleFor(x => x.CoverUrl).MaximumLength(256).WithMessage("封面图地址长度不能超过256！");

				RuleFor(x => x.Tags).ForEach(x => x.MaximumLength(32).WithMessage("单个标签长度不能超过32个字符！"))
					.Must(y => y.Count() <= 5).WithMessage("标签数量不能超过5个！");

				RuleFor(x => x.Slug).MaximumLength(128).WithMessage("缩略名长度不能超过128！");

				RuleFor(x => x.Slug)
					.Must(x => x.IsSlug())
					.WithMessage(x => $"缩略名{x.Slug}无效，只能为字母数字横线和下划线组合，且不能使用纯数字！")
					.When(x => !string.IsNullOrEmpty(x.Slug));
			}


		}

		private async Task<IResult> Handle([FromBody] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var id = 0;
			if (request.IsDraft)
			{
                var command =  mapper.Map<CreateDraftCommand>(request);
                var response = await mediator.Send(command, cancellationToken);
			}
			else
			{
                var command = mapper.Map<CreateArticleCommand>(request);
                var response = await mediator.Send(command, cancellationToken);


            }
		
            return Result.Ok(new { Id = id });
        }
	}

	internal class CreateArticleMappingProfile : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.ForType<CreateArticleEndpoint.Request, CreateArticleCommand>();
		}
	}
}
