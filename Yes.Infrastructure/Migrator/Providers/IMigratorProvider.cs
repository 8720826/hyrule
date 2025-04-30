namespace Yes.Infrastructure.Migrator.Providers
{
    public interface IMigratorProvider : IScoped
    {
        void MigrateUp(DatabaseTypeEnum databaseType, string connectionString);

        void MigrateUp(long? version = null, bool ensureDatabase = false);

        void MigrateDown(long version);
    }
}
