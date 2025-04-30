namespace Yes.Domain.Core.Exceptions
{

    public class EmptySlugException : BaseException
    {


        public EmptySlugException() : base($"缩略名不能为空！")
        {

        }
    }
}
