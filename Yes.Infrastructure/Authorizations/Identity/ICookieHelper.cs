namespace Yes.Infrastructure.Authorizations.Identity
{
    public interface ICookieHelper : IScoped
    {
        void SetCookie(string key, string value);

        void SetCookie(string key, string value, int expiresTime);

        string GetCookie(string key);

        void DeleteCookie(string key);
    }
}
