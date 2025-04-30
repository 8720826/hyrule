namespace Yes.Blog.Endpoints.Admins.Configurations
{
    public class GetConfigurationEndpoint : AdminEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapGet("/configuration", Handle);


		private async Task<IResult> Handle(
			IMediator mediator,
			IMapper mapper,
			CancellationToken cancellationToken)
		{
			var query = new GetConfigurationQuery();
			var response = await mediator.Send(query, cancellationToken);

			return Result.Ok(response);
		}
	}
}
