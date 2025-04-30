namespace Yes.Infrastructure.Migrator.Providers
{
    public class MigratorProvider : IMigratorProvider
    {
        private readonly ILogger<MigratorProvider> _logger;
        private readonly IConnectionStringProvider _connectionStringProvider;
        public MigratorProvider(ILogger<MigratorProvider> logger, IConnectionStringProvider connectionStringProvider)
        {
            _logger = logger;
            _connectionStringProvider = connectionStringProvider;
        }


        public void MigrateUp(DatabaseTypeEnum databaseType, string connectionString)
        {

            EnsureDatabase(databaseType, connectionString);
            using (var serviceProvider = MigratorHelper.CreateServices(databaseType, connectionString))
            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
        }


        public void MigrateUp(long? version = null, bool ensureDatabase = false)
        {
            var databaseType = _connectionStringProvider.GetDatabaseType();

            var connectionString = _connectionStringProvider.GetConnectionString();
            if (ensureDatabase)
            {
                EnsureDatabase(databaseType, connectionString);
            }



            using (var serviceProvider = MigratorHelper.CreateServices(databaseType, connectionString))
            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
        }


        public void MigrateDown(long version)
        {

            var connectionString = _connectionStringProvider.GetConnectionString();
            var databaseType = _connectionStringProvider.GetDatabaseType();

            using (var serviceProvider = MigratorHelper.CreateServices(databaseType, connectionString))
            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateDown(version);
            }

        }


        private void EnsureDatabase(DatabaseTypeEnum databaseType, string connectionString)
        {
            var connectionStringWithoutDatabase = _connectionStringProvider.GetConnectionStringWithoutDatabase(databaseType, connectionString);
            var dbName = _connectionStringProvider.GetDatabase(databaseType, connectionString);

            switch (databaseType)
            {
                case DatabaseTypeEnum.SqlServer:

                    var connection = new SqlConnection(connectionStringWithoutDatabase);
                    var records = connection.Query($"SELECT name FROM sys.databases WHERE name = '{dbName}';");
                    if (!records.Any())
                    {
                        connection.Execute($"CREATE DATABASE {dbName}");
                    }
                    break;



            }



        }

    }
}
