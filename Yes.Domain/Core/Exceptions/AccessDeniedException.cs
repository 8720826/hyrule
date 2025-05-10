namespace Yes.Domain.Core.Exceptions
{
    public class AccessDeniedException : BaseException
    {

        public AccessDeniedException()
            : base("访问被拒绝：没有足够的权限执行此操作。")
        {
        }

        public AccessDeniedException(string message)
            : base(message)
        {
        }

        public AccessDeniedException(string resourceName, string requiredPermission)
            : this($"访问被拒绝：对资源 '{resourceName}' 需要权限 '{requiredPermission}'。")
        {

        }
    }
}
