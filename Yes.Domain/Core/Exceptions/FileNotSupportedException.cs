namespace Yes.Domain.Core.Exceptions
{

    public class FileNotSupportedException : BaseException
    {
        public FileNotSupportedException(string message) : base($"模板文件压缩包格式错误！{message}")
        {

        }
        public FileNotSupportedException() : base($"模板文件压缩包格式错误！")
        {

        }
    }
}
