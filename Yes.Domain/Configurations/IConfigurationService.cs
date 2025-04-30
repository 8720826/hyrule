namespace Yes.Domain.Configurations
{
    public interface IConfigurationService : IScoped
    {
        Task SaveConfiguration(BlogSettings settings);
    }
}
