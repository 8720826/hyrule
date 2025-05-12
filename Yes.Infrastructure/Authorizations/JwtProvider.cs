namespace Yes.Infrastructure.Authorizations
{
    public class JwtProvider : IJwtProvider
    {
        private readonly BlogSettings _settings;

        public JwtProvider(IOptionsMonitor<BlogSettings> options)
        {
            _settings = options.CurrentValue;
        }



        public async Task<ClaimsPrincipal> ReadToken(string token)
        {
            return await Task.Run(() =>
            {
                var securityKeyBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(_settings.SecretKey));
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKeyBase64));

                var jwtHander = new JwtSecurityTokenHandler();
                var claimsIdentity = jwtHander.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "hyrule",
                    ValidAudience = "",
                    IssuerSigningKey = key,
                    ValidateLifetime = true,

                }, out _);
                return claimsIdentity;
            });
        }
    }
}
