namespace Yes.Application.Admins.Auths
{
    public record UserLoginCommand(string Name, string Password) : IRequest<UserLoginCommandResponse>;

    public record UserLoginCommandResponse(string Token, IdentityInfo Identity);

    public class UserLoginCommandHandler(BlogDbContext db) : IRequestHandler<UserLoginCommand, UserLoginCommandResponse>
    {
        private readonly BlogDbContext _db = db;

        public async Task<UserLoginCommandResponse> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = _db.Users.FirstOrDefault(x => x.Name == request.Name);
            if (user == null)
            {
                throw new InvalidPasswordException();
            }

            user.CheckPassword(request.Password);

            return new UserLoginCommandResponse("", new IdentityInfo
            {
                Name = user.Name,
                Id = user.Id,
                Role = user.IsSystem ? IdentityRoleEnum.Admin : IdentityRoleEnum.User
            });

        }
    }
}
