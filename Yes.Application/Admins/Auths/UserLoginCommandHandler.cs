namespace Yes.Application.Admins.Auths
{
    public record UserLoginCommand(string Name, string Password) : IRequest<UserLoginCommandResponse>;

    public record UserLoginCommandResponse(string Token, IdentityInfo Identity, int TokenLifetimeMinutes);

    public class UserLoginCommandHandler(BlogDbContext db, IOptionsMonitor<BlogSettings> options) : IRequestHandler<UserLoginCommand, UserLoginCommandResponse>
    {
        private readonly BlogDbContext _db = db;
        private readonly BlogSettings _settings = options.CurrentValue;
        public async Task<UserLoginCommandResponse> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = _db.Users.FirstOrDefault(x => x.Name == request.Name);
            if (user == null)
            {
                throw new InvalidPasswordException();
            }

            user.CheckPassword(request.Password);

            var role = user.IsSystem ? IdentityRoleEnum.Admin : IdentityRoleEnum.User;
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("Role", role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = JwtHelper.GenerateToken(claims, _settings.SecretKey, _settings.TokenLifetimeMinutes);

            return new UserLoginCommandResponse(token, new IdentityInfo
            {
                Name = user.Name,
                Id = user.Id,
                Role = role
            }, _settings.TokenLifetimeMinutes);

        }
    }
}
