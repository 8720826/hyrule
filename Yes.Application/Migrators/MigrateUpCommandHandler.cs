namespace Yes.Application.Migrators
{
    public record MigrateUpCommand(long? Version) : IRequest<Unit>;

    public class MigrateUpCommandHandler(IMigratorProvider migratorProvider) : IRequestHandler<MigrateUpCommand, Unit>
    {
        private readonly IMigratorProvider _migratorProvider = migratorProvider;

        public async Task<Unit> Handle(MigrateUpCommand request, CancellationToken cancellationToken)
        {
            _migratorProvider.MigrateUp(request.Version, ensureDatabase: true);

            return await Task.FromResult(new Unit());
        }
    }
}
