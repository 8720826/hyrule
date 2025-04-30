namespace Yes.Application.Admins.Users
{
    public record DeleteUserCommand(int Id) : IRequest<Unit>;

    public class DeleteUserCommandHandler(BlogDbContext db) : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly BlogDbContext _db = db;

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
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
