namespace Yes.Domain.Core.Exceptions
{

    public class StorageTypeException : BaseException
    {
        public StorageTypeException(string storageType) : base($"文件存储[{storageType}]配置错误！")
        {

        }
        public StorageTypeException() : base($"文件存储配置错误！")
        {

        }
    }
}
