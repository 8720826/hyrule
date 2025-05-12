

namespace Yes.Application.Admins.Auths
{
    public record ValidateCommand(string Token) : IRequest<ValidateCommandResponse>;

    public record ValidateCommandResponse(string Token, IdentityInfo Identity, int TokenLifetimeMinutes);

    public class ValidateCommandHandler(BlogDbContext db, IOptionsMonitor<BlogSettings> options) : IRequestHandler<ValidateCommand, ValidateCommandResponse>
    {
        private readonly BlogDbContext _db = db;
        private readonly BlogSettings _settings = options.CurrentValue;
        public async Task<ValidateCommandResponse> Handle(ValidateCommand request, CancellationToken cancellationToken)
        {
            var token = request.Token;
            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidTokenException();
            }

            try
            {
                var securityKeyBase64 = JwtHelper.GenerateKey(_settings.SecretKey ?? "");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKeyBase64));

                var jwtHander = new JwtSecurityTokenHandler();
                var claimsIdentity = jwtHander.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = false,
                    ValidIssuer = "",
                    ValidAudience = "",
                    IssuerSigningKey = key,
                    ValidateLifetime = true,

                }, out _);

                var id = int.Parse(claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");

                var user = await _db.Users.FindAsync(id);
                if (user == null)
                {
                    throw new InvalidTokenException();
                }
                var role = user.IsSystem ? IdentityRoleEnum.Admin : IdentityRoleEnum.User;

                return new ValidateCommandResponse(token, new IdentityInfo
                {
                    Name = user.Name,
                    Id = id,
                    Role = role
                }, _settings.TokenLifetimeMinutes);
            }
            catch (Exception ex)
            {
                throw new InvalidTokenException();
            }

        }
    }
}
