namespace Yes.Domain.Blogs
{
    public class CommentModel
    {
        public int Id { get; set; }


        public string Content { get; set; }

        public DateTime CreateDate { get; set; }

        public string Email { get; set; }

        public string Url { get; set; }

        public string IP { get; set; }

        public int UserId { get; set; }

        public string NickName { get; set; }

    }
}
