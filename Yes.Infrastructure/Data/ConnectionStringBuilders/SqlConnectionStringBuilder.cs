namespace Yes.Infrastructure.Data.ConnectionStringBuilders
{
    public class SqlConnectionStringBuilder : IConnectionStringBuilder
    {
        private readonly Microsoft.Data.SqlClient.SqlConnectionStringBuilder builder;

        public SqlConnectionStringBuilder()
        {
            builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder()
            {
                TrustServerCertificate = true
            };
        }

        public SqlConnectionStringBuilder(string connectionString)
        {
            builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder
            {
                ConnectionString = connectionString,
                TrustServerCertificate = true,
            };
        }

        public SqlConnectionStringBuilder(string dataSource, string initialCatalog, string userId, string password)
        {
            builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder()
            {
                DataSource = dataSource,
                InitialCatalog = initialCatalog,
                UserID = userId,
                Password = password,
                TrustServerCertificate = true
            };
        }

        public string GetConnectionString()
        {
            return builder.ConnectionString;
        }

        public string GetDatabaseName()
        {
            return builder.InitialCatalog ?? "";
        }

        public void SetDatabaseName(string name)
        {
            builder.InitialCatalog = name;
        }
    }
}
