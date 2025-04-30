namespace Yes.Blog.Endpoints.Admins.Storages
{
    public class UploadFileEndpoint : AdminEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapPost("/Storages/upload", Handle).DisableAntiforgery();
		public record Request(
			IFormFile File
		);


		private async Task<IResult> Handle(
			IFormFile File,
			IMediator mediator,
			IMapper mapper,
			CancellationToken cancellationToken)
		{
			var command = new UploadFileCommand(File);
			var response = await mediator.Send(command, cancellationToken);

			return Result.Ok(response);
		}
	}
}
