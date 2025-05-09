namespace Yes.Blog.Endpoints.Admins.Themes
{
    public class UploadThemeEndpoint : ThemeEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapPost("/themes/upload", Handle).DisableAntiforgery();


		private async Task<IResult> Handle(IFormFile File, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
            var command = new UploadThemeCommand(File);
			var response = await mediator.Send(command, cancellationToken);


			return Result.Ok(response);
		}
	}

}
