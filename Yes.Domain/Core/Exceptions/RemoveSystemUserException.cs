namespace Yes.Domain.Core.Exceptions
{
    public class RemoveSystemUserException : BaseException
    {

        public RemoveSystemUserException() : base($"系统管理员不能被删除！")
        {

        }
    }
}
