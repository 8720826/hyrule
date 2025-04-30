namespace Yes.Domain.Core.Exceptions
{

    public class ThemeNotExistsException : BaseException
    {
        public ThemeNotExistsException(string theme) : base($"主题{theme}不存在！")
        {

        }

        public ThemeNotExistsException() : base($"主题不存在！")
        {

        }
    }
}
