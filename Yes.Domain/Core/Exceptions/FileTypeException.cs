namespace Yes.Domain.Core.Exceptions
{

    public class FileTypeException : BaseException
    {
        public FileTypeException(string fileType) : base($"只能上传后缀名为{fileType}的文件！")
        {

        }
        public FileTypeException() : base($"该后缀名文件禁止上传！")
        {

        }
    }
}
