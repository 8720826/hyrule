namespace Yes.Infrastructure.Authorizations.User
{


    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        public UserAuthorizeAttribute()
        {
            AuthenticationSchemes = AuthenticationScheme.UserScheme;
        }
    }
}
