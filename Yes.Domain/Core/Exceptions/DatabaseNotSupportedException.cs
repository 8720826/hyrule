namespace Yes.Domain.Core.Exceptions
{

    public class DatabaseNotSupportedException : BaseException
    {
        public DatabaseNotSupportedException(string message) : base($"数据库配置错误，请检查！原因：{message}")
        {

        }

        public DatabaseNotSupportedException() : base($"数据库配置错误，请检查！")
        {

        }
    }
}
