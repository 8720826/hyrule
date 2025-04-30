namespace Yes.Application.Admins.Pages
{
	public record DeletePageCommand(int Id) : IRequest<DeletePageCommandResponse>;

    public record DeletePageCommandResponse(int Id);

    public class DeletePageCommandHandler(BlogDbContext db) : IRequestHandler<DeletePageCommand, DeletePageCommandResponse>
    {
        private readonly BlogDbContext _db = db;

        public async Task<DeletePageCommandResponse> Handle(DeletePageCommand request, CancellationToken cancellationToken)
        {
            var article = await _db.Articles.FindAsync(request.Id);
			if (article != null && article.IsPage())
			{
                article.Delete();
				_db.Articles.Update(article);
				await _db.SaveChangesAsync();
			}

			return new DeletePageCommandResponse(request.Id);
        }
    }
}
