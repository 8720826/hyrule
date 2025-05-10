namespace Yes.Application.Admins.Users
{
    public record CreateUserCommand(
        string Name,
        string Email,
        string NickName,
        string Password
    ) : IRequest<CreateUserCommandResponse>;

    public record CreateUserCommandResponse(int Id);

    public class CreateUserCommandHandler(BlogDbContext db, IIdentityContext identity) : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
    {
        private readonly BlogDbContext _db = db;
        private readonly IIdentityContext _identity = identity;

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var identityUser = await _db.Users.FindAsync(_identity.Id);
            if (identityUser == null)
            {
                throw new UserNotExistsException(_identity.Id);
            }

            if (!identityUser.IsSystemUser())
            {
                throw new AccessDeniedException();
            }


            var user = UserEntity.Create(request.Name, request.Email, request.NickName, request.Password);
			await _db.Users.AddAsync(user);
			await _db.SaveChangesAsync();

			return new CreateUserCommandResponse(user.Id);
        }
    }
}
