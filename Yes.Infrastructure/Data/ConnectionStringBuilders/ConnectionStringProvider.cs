namespace Yes.Infrastructure.Data.ConnectionStringBuilders
{
    public class ConnectionStringProvider(IOptionsMonitor<BlogSettings> options) : IConnectionStringProvider
    {
        private readonly BlogSettings _settings = options.CurrentValue;


        public string GetConnectionString(DatabaseTypeEnum databaseType, string server, string database, string user, string password)
        {
            var builder = ConnectionStringBuilderFactory.CreateStringBuilder(databaseType, server, database, user, password);
            return builder.GetConnectionString();
        }

        public string GetConnectionString()
        {
            var dbName = _settings.DatabaseName;
            var connectionString = _settings.ConnectionString;
            var databaseType = GetDatabaseType();

            var builder = ConnectionStringBuilderFactory.CreateStringBuilder(databaseType, connectionString);
            builder.SetDatabaseName(dbName);

            return builder.GetConnectionString();
        }

        public DatabaseTypeEnum GetDatabaseType()
        {
            Enum.TryParse<DatabaseTypeEnum>(_settings.DatabaseType, out var databaseType);
            return databaseType;
        }

        public string GetDatabaseVersion()
        {
            return _settings.DatabaseVersion;
        }

        public string GetConnectionStringWithoutDatabase(DatabaseTypeEnum databaseType, string connectionString)
        {
            var builder = ConnectionStringBuilderFactory.CreateStringBuilder(databaseType, connectionString);

            builder.SetDatabaseName("");
            return builder.GetConnectionString();
        }

        public string GetDatabase(DatabaseTypeEnum databaseType, string connectionString)
        {
            var builder = ConnectionStringBuilderFactory.CreateStringBuilder(databaseType, connectionString);

            return builder.GetDatabaseName();
        }
    }
}
