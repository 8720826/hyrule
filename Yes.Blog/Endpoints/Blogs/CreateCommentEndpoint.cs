namespace Yes.Blog.Endpoints.Blogs
{
    public class CreateCommentEndpoint : BlogEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapPost("/comments", Handle).WithRequestValidation<Request>();

        
        public record Request(
            int TargetId,
            string NickName,
            string Email,
            string Url,
			string Content
        );


        public class RequestValidator : AbstractValidator<Request>
        {
            public RequestValidator()
            {
                RuleFor(x => x.Content).NotEmpty().WithMessage("评论内容不能为空！");
                RuleFor(x => x.NickName).NotEmpty().WithMessage("昵称不能为空！");
                RuleFor(x => x.Email).NotEmpty().WithMessage("邮箱不能为空！");


                RuleFor(x => x.Email).MaximumLength(256).WithMessage("邮箱长度不能超过256！")
                                        .EmailAddress().WithMessage("邮箱格式错误！");

                RuleFor(x => x.Content).MinimumLength(10).WithMessage("评论内容长度不能少于10！")
                                        .MaximumLength(512).WithMessage("评论内容长度不能超过512！");

                RuleFor(x => x.NickName).MaximumLength(64).WithMessage("昵称长度不能超过64！");

                RuleFor(x => x.Url).MaximumLength(256).WithMessage("网址长度不能超过256！");

                RuleFor(x => x.Url).Must(x => x.IsUrlValid()).WithMessage("网址格式错误！").When(x => !string.IsNullOrEmpty(x.Url));
            }


        }

        private async Task<IResult> Handle(
            [FromBody] Request request, 
            IMediator mediator, 
            IMapper mapper,
            HttpContext httpContext,
			CancellationToken cancellationToken)
		{
            var redirectUrl = "/";
            var error = "";
            try
            {
                var command = mapper.Map<CreateCommentCommand>(request);
                command = command with
                {
                    Referer = httpContext.Request.Headers["Referer"].ToString(),
                    IP = httpContext.GetIpAddress()
                };

                var response = await mediator.Send(command, cancellationToken);

                if (!string.IsNullOrEmpty(command.Referer) && command.Referer.IsUrlValid())
                {
                    redirectUrl = command.Referer;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return Result.Ok(new { redirectUrl , error });
        }
	}
}
