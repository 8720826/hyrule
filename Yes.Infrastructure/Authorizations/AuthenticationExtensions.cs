namespace Yes.Infrastructure.Authorizations
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAdminAuthentication(this IServiceCollection services,IConfiguration configuration)
        {
            var securityKeyBase64 = JwtHelper.GenerateKey(configuration["SecretKey"] ?? "");


            services.AddAuthentication(AuthenticationScheme.AdminScheme)
              .AddCookie(AuthenticationScheme.AdminScheme, options =>
              {
                  options.Cookie.Name = AuthenticationScheme.AdminScheme;
                  options.LoginPath = "/admin/login";
                  options.LogoutPath = "/admin/logout";
                  options.ExpireTimeSpan = TimeSpan.FromDays(3);
                  options.SlidingExpiration = true;

                  options.Events.OnRedirectToAccessDenied =
                  options.Events.OnRedirectToLogin = c =>
                  {
                      if (c.Request.GetDisplayUrl().Contains("/api"))
                      {
                          c.Response.ContentType = "application/json";
                          c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                          return Task.FromResult(Result.Error("请登录后操作！"));
                      }
                      else
                      {
                          c.Response.Redirect(c.RedirectUri);
                          return Task.CompletedTask;
                      }
                  };
              }).AddJwtBearer(AuthenticationScheme.ApiScheme, options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateLifetime = true,
                      RequireSignedTokens = false,
                      RequireExpirationTime = false,
                      ValidateIssuer = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = "hyrule",
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKeyBase64))
                  };

              });

            return services;
        }



        public static async Task SignIn(this HttpContext context, IdentityInfo account, int tokenLifetimeMinutes)
        {
            await context.SignIn(AuthenticationScheme.AdminScheme, account, tokenLifetimeMinutes);
        }

        public static async Task SignIn(this HttpContext context, string authenticationScheme, IdentityInfo account, int tokenLifetimeMinutes)
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
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(tokenLifetimeMinutes),//有效时间
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
            await context.SignOutAsync(AuthenticationScheme.AdminScheme);
        }

        public static async Task<bool> HasLogin(this HttpContext context)
        {
            return await Task.FromResult(context.User.Identity?.IsAuthenticated ?? false);
        }
    }
}
