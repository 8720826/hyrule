namespace Yes.Blog.Endpoints.Migrators
{
	public class MigrateUpEndpoint : MigratorEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapGet("/up", Handle);

		public record Request(long? Version);

		private async Task<Microsoft.AspNetCore.Http.IResult> Handle(
		  [AsParameters] Request request,
		  IMediator mediator,
		  IMapper mapper,
		  CancellationToken cancellationToken)
		{
			var command = new MigrateUpCommand(request.Version);
			var response = await mediator.Send(command, cancellationToken);

			return Results.Ok("up");
		}
	}
}
