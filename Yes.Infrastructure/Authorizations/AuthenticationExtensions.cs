namespace Yes.Infrastructure.Authorizations
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddUserAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(AuthenticationScheme.UserScheme)
              .AddCookie(AuthenticationScheme.UserScheme, options =>
              {
                  options.Cookie.Name = AuthenticationScheme.UserScheme;
                  options.LoginPath = "/admin/login";
                  options.LogoutPath = "/admin/logout";
                  options.ExpireTimeSpan = TimeSpan.FromDays(3);
                  options.SlidingExpiration = true;
              });
            return services;
        }

        public static async Task SignIn(this HttpContext context, IdentityInfo account)
        {
            await context.SignIn(AuthenticationScheme.UserScheme, account);
        }

        public static async Task SignIn(this HttpContext context, string authenticationScheme, IdentityInfo account)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, account.Name),
                new("Id", account.Id.ToString()),
                new("Name", account.Name),
                new("Role", account.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(authenticationScheme);
            claimsIdentity.AddClaims(claims);

            await context.SignInAsync(authenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24 * 30),//有效时间
                    AllowRefresh = true
                }
            );


        }

        public static async Task SignOut(this HttpContext context, string authenticationScheme)
        {
            await context.SignOutAsync(authenticationScheme);
        }

        public static async Task SignOut(this HttpContext context)
        {
            await context.SignOutAsync(AuthenticationScheme.UserScheme);
        }

        public static async Task<bool> HasLogin(this HttpContext context)
        {
            return await Task.FromResult(context.User.Identity?.IsAuthenticated ?? false);
        }
    }
}
