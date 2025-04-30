namespace Yes.Domain.Core.Exceptions
{
    public class CommentNotExistsException : BaseException
    {
        public CommentNotExistsException(int commentId) : base($"找不到id为{commentId}的评论！")
        {

        }

        public CommentNotExistsException() : base($"评论不存在！")
        {

        }
    }
}
