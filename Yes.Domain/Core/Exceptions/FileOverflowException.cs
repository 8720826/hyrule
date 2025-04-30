namespace Yes.Domain.Core.Exceptions
{

    public class FileOverflowException : BaseException
    {
        public FileOverflowException(string maxSize) : base($"文件大小超出限制{maxSize}！")
        {

        }
        public FileOverflowException() : base($"文件大小超出限制！")
        {

        }
    }
}
