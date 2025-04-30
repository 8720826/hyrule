namespace Yes.Domain.Core.Exceptions
{
    public class ArticleNotExistsException : BaseException
    {
        public ArticleNotExistsException(int articleId) : base($"找不到id为{articleId}的文章！")
        {

        }

        public ArticleNotExistsException() : base($"文章不存在！")
        {

        }
    }
}
