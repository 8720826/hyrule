namespace Yes.Blog.Endpoints.Admins.Comments
{
    public class GetCommentsEndpoint : CommentEndpointScheme, IEndpoint
    {
        public void Map(IEndpointRouteBuilder app) => app.MapGet("/comments", Handle);

        public record Request(int? PageIndex);


        private async Task<IResult> Handle(
            [AsParameters] Request request,
            IMediator mediator,
            IMapper mapper,
            CancellationToken cancellationToken)
        {
            var query = new GetCommentsQuery(request.PageIndex, 15);
            var response = await mediator.Send(query, cancellationToken);

            return Result.Ok(response);
        }
    }
}
