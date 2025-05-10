namespace Yes.Application.Admins.Users
{
    public record DeleteUserCommand(int Id) : IRequest<Unit>;

    public class DeleteUserCommandHandler(BlogDbContext db, IIdentityContext identity) : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly BlogDbContext _db = db;
        private readonly IIdentityContext _identity = identity;
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
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
				throw new RemoveSystemUserException();
			}

			_db.Users.Remove(user);
			await _db.SaveChangesAsync();

			return new Unit();
        }
    }
}
