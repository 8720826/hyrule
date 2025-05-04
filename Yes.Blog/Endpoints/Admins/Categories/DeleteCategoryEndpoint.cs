namespace Yes.Blog.Endpoints.Admins.Categories
{
    public class DeleteCategoryEndpoint : CategoryEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapDelete("/categories", Handle);

		public record Request(int Id);


		private async Task<IResult> Handle([AsParameters] Request request, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
		{
			var command = mapper.Map<DeleteCategoryCommand>(request);
			var response = await mediator.Send(command, cancellationToken);


			return Result.Ok(response);
		}
	}


}
