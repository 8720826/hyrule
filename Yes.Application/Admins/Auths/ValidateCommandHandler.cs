
namespace Yes.Application.Admins.Auths
{
    public record ValidateCommand(string Token) : IRequest<ValidateCommandResponse>;

    public record ValidateCommandResponse(string Token, IdentityInfo Identity, int TokenLifetimeMinutes);

    public class ValidateCommandHandler(BlogDbContext db, IOptionsMonitor<BlogSettings> options, IJwtProvider jwtProvider) : IRequestHandler<ValidateCommand, ValidateCommandResponse>
    {
        private readonly BlogDbContext _db = db;
        private readonly BlogSettings _settings = options.CurrentValue;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        public async Task<ValidateCommandResponse> Handle(ValidateCommand request, CancellationToken cancellationToken)
        {
            var token = request.Token;
            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidTokenException();
            }

            try
            {
                var claimsIdentity = await _jwtProvider.ReadToken(token);

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
