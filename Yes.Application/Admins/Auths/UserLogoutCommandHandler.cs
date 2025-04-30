namespace Yes.Application.Admins.Auths
{
    public record UserLogoutCommand() : IRequest<Unit>;


    public class UserLogoutCommandHandler(BlogDbContext db) : IRequestHandler<UserLogoutCommand, Unit>
    {
        private readonly BlogDbContext _db = db;

        public async Task<Unit> Handle(UserLogoutCommand request, CancellationToken cancellationToken)
        {
            //TODO 清除token
            return new Unit();
        }
    }
}
