namespace Yes.Blog.Endpoints.Admins.Configurations
{
    public class GetConfigurationEndpoint : ConfigurationEndpointScheme, IEndpoint
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
