

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

                    using (var connection = new SqlConnection(connectionStringWithoutDatabase))
                    {
                        var records = connection.Query($"SELECT name FROM sys.databases WHERE name = '{dbName}';");
                        if (!records.Any())
                        {
                            connection.Execute($"CREATE DATABASE {dbName}");
                        }
                    }
                    break;

                case DatabaseTypeEnum.PostgreSQL:

                    using (var connection = new NpgsqlConnection(connectionStringWithoutDatabase))
                    {
                        var exists = connection.ExecuteScalar<bool>(
                            "SELECT EXISTS(SELECT 1 FROM pg_database WHERE datname = @dbName)",
                            new { dbName }
                        );
                        if (!exists)
                        {
                            connection.Execute($"CREATE DATABASE \"{dbName}\" ENCODING = 'UTF8'");
                        }
                    }
                    break;

                case DatabaseTypeEnum.MySql:
                    using (var connection = new MySqlConnection(connectionStringWithoutDatabase)) // 需要 MySql.Data NuGet 包
                    {
                        // 参数化查询
                        var records = connection.Query<string>(
                            "SELECT SCHEMA_NAME FROM information_schema.SCHEMATA WHERE SCHEMA_NAME = @dbName",
                            new { dbName }
                        );

                        if (!records.Any())
                        {
                            // 指定字符集和排序规则（推荐）
                            connection.Execute($@"
                        CREATE DATABASE `{dbName}` 
                        CHARACTER SET utf8mb4 
                        COLLATE utf8mb4_general_ci");
                        }
                    }
                    break;


            }



        }

    }
}
