namespace Yes.Infrastructure.Authorizations.Identity.Context
{
    public class IdentityInfo
    {

        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;


        public IdentityRoleEnum Role { get; set; }
    }
}
