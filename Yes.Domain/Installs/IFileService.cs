namespace Yes.Domain.Installs
{
    public interface IFileService : IScoped
    {
        void CopyFolder(string sources, string dest);
    }
}
