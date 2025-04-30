namespace Yes.Infrastructure.Authorizations
{
    public class JwtProvider : IJwtProvider
    {
        private readonly string securityKey = "";

        public JwtProvider(IConfiguration configuration)
        {
            securityKey = configuration.GetSection("Jwt").GetValue<string>("securityKey");
        }



        public async Task<ClaimsPrincipal> ReadToken(string token)
        {
            return await Task.Run(() =>
            {
                var securityKeyBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(securityKey));
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKeyBase64));

                var jwtHander = new JwtSecurityTokenHandler();
                var claimsIdentity = jwtHander.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "",
                    ValidAudience = "",
                    IssuerSigningKey = key,
                    ValidateLifetime = true,

                }, out _);
                return claimsIdentity;
            });
        }
    }
}
