using Yes.Blog.Endpoints.Admins;

namespace Yes.Blog.Endpoints.Blogs
{
    public class InstallEndpoint : BlogEndpointScheme, IEndpoint
    {
        public void Map(IEndpointRouteBuilder app) => app.MapPost("install", Handle).WithRequestValidation<Request>();

        public record Request(
            string DatabaseType,
            string DatabaseServer,
            string DatabaseVersion,
            string DatabaseUser,
            string DatabasePassword,
            string DatabaseName,
            bool UseDefaultAdmin
        );


        public class RequestValidator : AbstractValidator<Request>
        {
            public RequestValidator()
            {
                RuleFor(x => x.DatabaseName).NotEmpty().WithMessage("数据库名不能为空！");
                RuleFor(x => x.DatabaseServer).NotEmpty().WithMessage("数据库地址不能为空！");
                RuleFor(x => x.DatabaseUser).NotEmpty().WithMessage("数据库用户不能为空！");
                RuleFor(x => x.DatabasePassword).NotEmpty().WithMessage("数据库密码不能为空！");

            }
        }


        private async Task<IResult> Handle(
            [FromBody] Request request,
            IMediator mediator,
            IMapper mapper,
            CancellationToken cancellationToken)
        {
            var command = mapper.Map<InstallCommand>(request);
            var response = await mediator.Send(command, cancellationToken);

            if (response.Success)
            {
                return Result.Ok();
            }
            else
            {
                return Result.Error(response.ErrorMessage);
            }
        }

    }
}
