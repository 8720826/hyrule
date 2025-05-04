namespace Yes.Blog.Endpoints.Admins.Comments
{
    public class DeleteCommentEndpoint : CommentEndpointScheme, IEndpoint
    {
        public void Map(IEndpointRouteBuilder app) => app.MapDelete("/comments", Handle);

        public record Request(int Id);


        private async Task<IResult> Handle([AsParameters] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
        {
            var command = new DeleteCommentCommand(request.Id);
            var response = await mediator.Send(command, cancellationToken);


            return Result.Ok(response);
        }
    }


}
