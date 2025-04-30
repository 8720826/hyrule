namespace Yes.Domain.Comments
{
    [Table("Comment")]
    public class CommentEntity
    {
        [Key]
        public int Id { get; set; }

        public int ArticleId { get; set; }

        public string Content { get; set; }

        public DateTime CreateDate { get; set; }

        public string Email { get; set; }

        public string Url { get; set; }

        public string IP { get; set; }

        public string Referer { get; set; }

        public int UserId { get; set; }

        public string NickName { get; set; }

        public CommentStatusEnum Status { get; set; }

        public static CommentEntity Create(int articleId, int userId, string nickName, string content, string email, string url, string ip, string referer)
        {
            return new CommentEntity
            {
                ArticleId = articleId,
                Content = content,
                CreateDate = DateTime.Now,
                Status = CommentStatusEnum.待审核,
                Email = email,
                NickName = nickName,
                UserId = userId,
                Url = url,
                IP = ip,
                Referer = referer
            };
        }

        public bool IsVerified()
        {
            return Status == CommentStatusEnum.审核通过;
        }

        public void Verify()
        {
            Status = CommentStatusEnum.审核通过;
        }

        public void UnVerify()
        {
            Status = CommentStatusEnum.待审核;
        }
    }
}
