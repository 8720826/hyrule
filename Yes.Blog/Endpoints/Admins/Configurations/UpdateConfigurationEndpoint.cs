namespace Yes.Blog.Endpoints.Admins.Configurations
{
    public class UpdateConfigurationEndpoint : ConfigurationEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapPut("/configuration", Handle).WithRequestValidation<Request>();

		public record Request(
			string Name,
			string Url,
			string Logo,
			string Keywords,
			string Description,
			string BeiAn,
			string Script,
			int PageSizeOfHomepage,
			int PageSizeOfListpage,
			string ArticleRoute,
			string PageRoute,
			string CategoryRoute,
            string SearchRoute,
            string TagRoute,
            StorageSettings Storage
		);

		public class RequestValidator : AbstractValidator<Request>
		{
			public RequestValidator()
			{
                RuleFor(x => x.Name).NotEmpty().WithMessage("站点名不能为空！");

                RuleFor(x => x.ArticleRoute).NotEmpty().WithMessage("文章路径不能为空！");
                RuleFor(x => x.PageRoute).NotEmpty().WithMessage("独立页面路径不能为空！");
                RuleFor(x => x.CategoryRoute).NotEmpty().WithMessage("分类页路径不能为空！");
                RuleFor(x => x.SearchRoute).NotEmpty().WithMessage("搜索页路径不能为空！");
                RuleFor(x => x.TagRoute).NotEmpty().WithMessage("标签页路径不能为空！");
            }
		}

		private async Task<IResult> Handle([FromBody] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var command = mapper.Map<UpdateConfigurationCommand>(request);
			var response = await mediator.Send(command, cancellationToken);


			return Result.Ok(response);
		}
	}

	internal class UpdateConfigurationMappingProfile : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.ForType<UpdateConfigurationEndpoint.Request, UpdateConfigurationCommand>();
		}
	}
}
