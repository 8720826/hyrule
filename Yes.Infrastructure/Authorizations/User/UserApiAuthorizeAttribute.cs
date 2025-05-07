namespace Yes.Infrastructure.Authorizations.User
{


    public class UserApiAuthorizeAttribute : AuthorizeAttribute
    {
        public UserApiAuthorizeAttribute()
        {
            AuthenticationSchemes = AuthenticationScheme.UserApiScheme;
        }
    }
}
