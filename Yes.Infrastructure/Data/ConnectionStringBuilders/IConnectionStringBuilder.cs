namespace Yes.Infrastructure.Data.ConnectionStringBuilders
{
    public interface IConnectionStringBuilder
    {
        string GetConnectionString();

        string GetDatabaseName();

        void SetDatabaseName(string name);
    }
}
