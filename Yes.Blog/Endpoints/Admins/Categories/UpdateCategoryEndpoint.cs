﻿namespace Yes.Blog.Endpoints.Admins.Categories
{
    public class UpdateCategoryEndpoint : CategoryEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapPut("/categories", Handle).WithRequestValidation<Request>();

        public record Request(
			int Id,
			string Name,
			string? Slug,
			string? CoverUrl,
			string? Description,
			int Sort
		);

		public class RequestValidator : AbstractValidator<Request>
		{
			public RequestValidator()
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("分类名不能为空！");
                RuleFor(x => x.Slug).NotEmpty().WithMessage("缩略名不能为空！");
            }
		}

		private async Task<IResult> Handle([FromBody] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var command = mapper.Map<UpdateCategoryCommand>(request);
			var response = await mediator.Send(command, cancellationToken);


			return Result.Ok(response);
		}
	}

	internal class UpdateCategoryMappingProfile : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.ForType<UpdateCategoryEndpoint.Request, UpdateCategoryCommand>();
		}
	}
}
