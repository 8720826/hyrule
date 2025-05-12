namespace Yes.Infrastructure.Authorizations.User
{


    public class PageAuthorizeAttribute : AuthorizeAttribute
    {
        public PageAuthorizeAttribute()
        {
            AuthenticationSchemes = "admin";
        }
    }
}
