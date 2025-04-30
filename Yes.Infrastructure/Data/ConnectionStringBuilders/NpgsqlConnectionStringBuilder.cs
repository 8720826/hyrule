namespace Yes.Infrastructure.Data.ConnectionStringBuilders
{
    public class NpgsqlConnectionStringBuilder : IConnectionStringBuilder
    {
        private readonly Npgsql.NpgsqlConnectionStringBuilder builder;

        public NpgsqlConnectionStringBuilder()
        {
            builder = new Npgsql.NpgsqlConnectionStringBuilder
            {

            };
        }

        public NpgsqlConnectionStringBuilder(string connectionString)
        {
            builder = new Npgsql.NpgsqlConnectionStringBuilder
            {
                ConnectionString = connectionString
            };
        }

        public NpgsqlConnectionStringBuilder(string host, string database, string userId, string password)
        {
            builder = new Npgsql.NpgsqlConnectionStringBuilder
            {
                Host = host,
                Database = database,
                Username = userId,
                Password = password
            };

        }

        public string GetConnectionString()
        {
            return builder.ConnectionString;
        }

        public string GetDatabaseName()
        {
            return builder.Database ?? "";
        }

        public void SetDatabaseName(string name)
        {
            builder.Database = name;
        }
    }
}
