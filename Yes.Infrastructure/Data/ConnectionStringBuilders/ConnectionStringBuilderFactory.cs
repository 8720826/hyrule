namespace Yes.Infrastructure.Data.ConnectionStringBuilders
{
    public class ConnectionStringBuilderFactory
    {
        public static IConnectionStringBuilder CreateStringBuilder(DatabaseTypeEnum databaseType)
        {
            switch (databaseType)
            {
                case DatabaseTypeEnum.SqlServer:
                    return new SqlConnectionStringBuilder();
                case DatabaseTypeEnum.MySql:
                    return new MySqlConnectionStringBuilder();
                case DatabaseTypeEnum.PostgreSQL:
                    return new NpgsqlConnectionStringBuilder();

                default:
                    throw new DatabaseNotSupportedException();
            }
        }

        public static IConnectionStringBuilder CreateStringBuilder(DatabaseTypeEnum databaseType, string connectionString)
        {
            switch (databaseType)
            {
                case DatabaseTypeEnum.SqlServer:
                    return new SqlConnectionStringBuilder(connectionString);
                case DatabaseTypeEnum.MySql:
                    return new MySqlConnectionStringBuilder(connectionString);
                case DatabaseTypeEnum.PostgreSQL:
                    return new NpgsqlConnectionStringBuilder(connectionString);

                default:
                    throw new DatabaseNotSupportedException();
            }
        }

        public static IConnectionStringBuilder CreateStringBuilder(DatabaseTypeEnum databaseType, string dataSource, string database, string userId, string password)
        {
            switch (databaseType)
            {
                case DatabaseTypeEnum.SqlServer:
                    return new SqlConnectionStringBuilder(dataSource, database, userId, password);
                case DatabaseTypeEnum.MySql:
                    return new MySqlConnectionStringBuilder(dataSource, database, userId, password);
                case DatabaseTypeEnum.PostgreSQL:
                    return new NpgsqlConnectionStringBuilder(dataSource, database, userId, password);


                default:
                    throw new DatabaseNotSupportedException();
            }
        }
    }
}
