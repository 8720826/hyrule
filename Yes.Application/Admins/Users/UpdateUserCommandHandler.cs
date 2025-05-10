namespace Yes.Application.Admins.Users
{
    public record UpdateUserCommand(
        int Id,
        string Name,
        string Email,
        string NickName,
        string Password
    ) : IRequest<Unit>;


    public class UpdateUserCommandHandler(BlogDbContext db, IIdentityContext identity) : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly BlogDbContext _db = db;
        private readonly IIdentityContext _identity = identity;
        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
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

            var user = await _db.Users.FindAsync(request.Id);
            if (user == null)
            {
                throw new UserNotExistsException(request.Id);
            }
            if (user.IsSystemUser())
            {
                throw new AccessDeniedException();
            }


            user.Update(request.Name, request.Email, request.NickName, request.Password);
            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return new Unit();
        }
    }
}
