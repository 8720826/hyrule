namespace Yes.Domain.Core.Exceptions
{

    public class SlugInUseException : BaseException
    {
        public SlugInUseException(string slug) : base($"缩略名{slug}已经被使用！")
        {

        }

        public SlugInUseException() : base($"缩略名已经被使用！")
        {

        }
    }
}
