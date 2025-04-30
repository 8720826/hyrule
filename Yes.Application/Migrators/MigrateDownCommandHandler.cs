namespace Yes.Application.Migrators
{
    public record MigrateDownCommand(long Version) : IRequest<Unit>;

    public class MigrateDownCommandHandler(IMigratorProvider migratorProvider) : IRequestHandler<MigrateDownCommand, Unit>
    {
        private readonly IMigratorProvider _migratorProvider = migratorProvider;

        public async Task<Unit> Handle(MigrateDownCommand request, CancellationToken cancellationToken)
        {
            _migratorProvider.MigrateDown(request.Version);

            return await Task.FromResult(new Unit());
        }
    }
}
