



namespace Yes.Blog.Endpoints.Admins.Articles
{
    public class DeleteArticleEndpoint : AdminEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapDelete("/articles", Handle);

		public record Request(int Id);



		private async Task<IResult> Handle([AsParameters] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var command = new DeleteArticleCommand(request.Id);
			var response = await mediator.Send(command, cancellationToken);


			return Result.Ok(response);
		}
	}


}
