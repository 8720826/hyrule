namespace Yes.Infrastructure.Authorizations.User
{


    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        public ApiAuthorizeAttribute()
        {
            AuthenticationSchemes = AuthenticationScheme.ApiScheme;
        }
    }
}
