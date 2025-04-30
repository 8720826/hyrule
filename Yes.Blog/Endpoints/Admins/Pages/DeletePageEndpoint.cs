namespace Yes.Blog.Endpoints.Admins.Pages
{
    public class DeletePageEndpoint : AdminEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapDelete("/pages", Handle);

		public record Request(int Id);

		private async Task<IResult> Handle([AsParameters] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var command = new DeletePageCommand(request.Id);
			var response = await mediator.Send(command, cancellationToken);


			return Result.Ok(response);
		}
	}


}
