namespace Yes.Domain.Core.Exceptions
{
    public class PageNotExistsException : BaseException
    {
        public PageNotExistsException(int articleId) : base($"找不到id为{articleId}的单页！")
        {

        }

        public PageNotExistsException() : base($"单页不存在！")
        {

        }
    }
}
