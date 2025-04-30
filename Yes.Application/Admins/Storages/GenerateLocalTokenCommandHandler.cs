namespace Yes.Application.Admins.Storages
{
    public record GenerateLocalTokenCommand(StorageTypeEnum StorageType) : IRequest<GenerateLocalTokenCommandResponse>;

    public record GenerateLocalTokenCommandResponse(string StorageType, string Token, string Prefix);
    public class GenerateLocalTokenCommandHandler(BlogDbContext db) : IRequestHandler<GenerateLocalTokenCommand, GenerateLocalTokenCommandResponse>
    {
        private readonly BlogDbContext _db = db;

        public async Task<GenerateLocalTokenCommandResponse> Handle(GenerateLocalTokenCommand request, CancellationToken cancellationToken)
        {

            var prefix = $"/";

            return new GenerateLocalTokenCommandResponse(request.StorageType.ToString(), "", prefix);
        }
    }
}
