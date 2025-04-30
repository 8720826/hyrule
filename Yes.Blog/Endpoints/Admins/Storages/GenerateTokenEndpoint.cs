

namespace Yes.Blog.Endpoints.Admins.Storages
{
    public class GenerateTokenEndpoint : AdminEndpointScheme, IEndpoint
	{
		public void Map(IEndpointRouteBuilder app) => app.MapPost("/Storages/token", Handle);

		private async Task<IResult> Handle(
			IMediator mediator,
			IMapper mapper,
			IOptionsMonitor<BlogSettings> options,
			CancellationToken cancellationToken)
		{
			Enum.TryParse(options.CurrentValue.Storage.StorageType, out StorageTypeEnum storageType);

			var response = await mediator.Send(storageType switch
			{
				StorageTypeEnum.Qiniu => new GenerateQiniuTokenCommand(storageType),
				_ => new GenerateLocalTokenCommand(storageType)
			}, cancellationToken);

			return Result.Ok(response);
		}
	}
}
