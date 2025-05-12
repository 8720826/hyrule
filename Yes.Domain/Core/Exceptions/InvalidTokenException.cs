namespace Yes.Domain.Core.Exceptions
{
    public class InvalidTokenException : BaseException
    {

        public InvalidTokenException() : base($"登录失效，请重新登录！")
        {

        }
    }
}
