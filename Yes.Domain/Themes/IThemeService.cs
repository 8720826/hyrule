namespace Yes.Domain.Themes
{
    public interface IThemeService:IScoped
    {
        void CheckThemeExists(string theme);
    }
}
