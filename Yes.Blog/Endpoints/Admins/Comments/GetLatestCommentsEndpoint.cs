namespace Yes.Blog.Endpoints.Admins.Comments
{
    public class GetLatestCommentsEndpoint : AdminEndpointScheme, IEndpoint
    {
        public void Map(IEndpointRouteBuilder app) => app.MapGet("/comments/Latest", Handle);



        private async Task<IResult> Handle(
            IMediator mediator,
            IMapper mapper,
            CancellationToken cancellationToken)
        {
            var query = new GetLatestCommentsQuery();
            var response = await mediator.Send(query, cancellationToken);

            return Result.Ok(response);
        }
    }
}
