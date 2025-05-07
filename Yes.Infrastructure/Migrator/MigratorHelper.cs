namespace Yes.Infrastructure.Migrator
{
    public class MigratorHelper
    {
        public static ServiceProvider CreateServices(DatabaseTypeEnum databaseType, string connectionString)
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddDatabase(databaseType)
                    // Set the connection string
                    .WithGlobalConnectionString(connectionString)
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(InitTables).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }
    }

    public static class MigratorExtensions
    {
        public static IMigrationRunnerBuilder AddDatabase(this IMigrationRunnerBuilder builder, DatabaseTypeEnum databaseType)
        {
            return databaseType switch
            {
                DatabaseTypeEnum.SqlServer => builder.AddSqlServer(),
                DatabaseTypeEnum.MySql => builder.AddMySql(),
                DatabaseTypeEnum.PostgreSQL => builder.AddPostgres(),

                _ => throw new DatabaseNotSupportedException()
            };
        }
    }
}
