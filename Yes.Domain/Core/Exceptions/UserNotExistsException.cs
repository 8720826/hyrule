namespace Yes.Domain.Core.Exceptions
{
    public class UserNotExistsException : BaseException
    {
        public UserNotExistsException(int userId) : base($"用户id{userId}不存在！")
        {

        }

        public UserNotExistsException() : base($"用户不存在！")
        {

        }
    }
}
