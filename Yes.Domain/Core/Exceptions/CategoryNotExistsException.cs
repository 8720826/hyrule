namespace Yes.Domain.Core.Exceptions
{
    public class CategoryNotExistsException : Exception
    {
        public CategoryNotExistsException(int categoryId) : base($"分类id{categoryId}不存在！")
        {

        }

        public CategoryNotExistsException() : base($"分类不存在！")
        {

        }
    }
}
