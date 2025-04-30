namespace Yes.Domain.Core.Exceptions
{
    public class InvalidPasswordException : BaseException
    {

        public InvalidPasswordException() : base($"用户名或密码错误！")
        {

        }
    }
}
