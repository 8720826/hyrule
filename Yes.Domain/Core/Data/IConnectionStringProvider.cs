
namespace Yes.Domain.Core.Data
{
    public interface IConnectionStringProvider : IScoped
    {
        string GetConnectionString(DatabaseTypeEnum databaseType, string server, string database, string user, string password);
        string GetConnectionString();

        DatabaseTypeEnum GetDatabaseType();



        string GetConnectionStringWithoutDatabase(DatabaseTypeEnum databaseType, string connectionString);

        string GetDatabase(DatabaseTypeEnum databaseType, string connectionString);


    }

}
