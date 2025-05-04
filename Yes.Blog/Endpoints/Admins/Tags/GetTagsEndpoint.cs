namespace Yes.Blog.Endpoints.Admins.Tags
{
    public class GetTagsEndpoint : TagEndpointScheme, IEndpoint
    {
        public void Map(IEndpointRouteBuilder app) => app.MapGet("/tags", Handle);

        public record Request(int? PageIndex);


        private async Task<IResult> Handle(
            [AsParameters] Request request,
            IMediator mediator,
            IMapper mapper,
            CancellationToken cancellationToken)
        {
            var query = new GetTagsQuery(request.PageIndex, 15);
            var response = await mediator.Send(query, cancellationToken);

            return Result.Ok(response);
        }
    }
}
