namespace Yes.Blog.Endpoints.Admins.Statistics
{
    public class GetStatsEndpoint : AdminEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapGet("/Stats", Handle);


		private async Task<IResult> Handle(IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var query = new GetStatsQuery();
			var response = await mediator.Send(query, cancellationToken);


			return Result.Ok(response);
		}
	}

}
