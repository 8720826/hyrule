namespace Yes.Blog.Endpoints.Admins.Comments
{
    public class UpdateCommentEndpoint : AdminEndpointScheme, IEndpoint
    {
        public void Map(IEndpointRouteBuilder app) => app.MapPut("/comments", Handle);

        public record Request(int Id);

        public class RequestValidator : AbstractValidator<Request>
        {
            public RequestValidator()
            {
            }
        }

        private async Task<IResult> Handle([AsParameters] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
        {
            var command = new UpdateCommentCommand(request.Id);
            var response = await mediator.Send(command, cancellationToken);


            return Result.Ok(response);
        }
    }


}
