

namespace Yes.Blog.Endpoints.Admins.Tags
{
    public class UpdateTagEndpoint : AdminEndpointScheme, IEndpoint
    {
        public void Map(IEndpointRouteBuilder app) => app.MapPut("/tags", Handle).WithRequestValidation<Request>();

        public record Request(
            int Id,
            string? Slug
        );

        public class RequestValidator : AbstractValidator<Request>
        {
            public RequestValidator()
            {
                RuleFor(x => x.Slug).NotEmpty().WithMessage("缩略名不能为空！");
            }
        }

        private async Task<IResult> Handle([FromBody] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
        {
            var command = mapper.Map<UpdateTagCommand>(request);
            var response = await mediator.Send(command, cancellationToken);


            return Result.Ok(response);
        }
    }
    internal class UpdateTagMappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<UpdateTagEndpoint.Request, UpdateTagCommand>();
        }
    }
}
