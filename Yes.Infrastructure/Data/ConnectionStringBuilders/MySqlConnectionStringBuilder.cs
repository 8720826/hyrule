namespace Yes.Infrastructure.Data.ConnectionStringBuilders
{
    public class MySqlConnectionStringBuilder : IConnectionStringBuilder
    {
        private readonly MySql.Data.MySqlClient.MySqlConnectionStringBuilder builder;

        public MySqlConnectionStringBuilder()
        {
            builder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder
            {

            };
        }

        public MySqlConnectionStringBuilder(string connectionString)
        {
            builder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder
            {
                ConnectionString = connectionString
            };
        }

        public MySqlConnectionStringBuilder(string server, string database, string userId, string password)
        {
            builder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder
            {
                Server = server,
                Database = database,
                UserID = userId,
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
