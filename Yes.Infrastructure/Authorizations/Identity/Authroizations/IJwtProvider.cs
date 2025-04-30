namespace Yes.Infrastructure.Authorizations.Identity.Authroizations
{
    public interface IJwtProvider : IScoped
    {
        Task<ClaimsPrincipal> ReadToken(string token);
    }
}
