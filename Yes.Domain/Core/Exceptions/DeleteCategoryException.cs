namespace Yes.Domain.Core.Exceptions
{
    public class DeleteCategoryException : BaseException
    {
        public DeleteCategoryException(string message) : base(message)
        {

        }

        public DeleteCategoryException() : base($"删除分类失败！")
        {

        }
    }
}
