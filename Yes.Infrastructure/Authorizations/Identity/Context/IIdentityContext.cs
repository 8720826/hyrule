namespace Yes.Infrastructure.Authorizations.Identity.Context
{
    public interface IIdentityContext : IScoped
    {
        int Id { get; }

        string Name { get; }



        IdentityRoleEnum Role { get; }


        bool IsAuthenticated();

        IEnumerable<Claim> GetClaimsIdentity();
    }
}
