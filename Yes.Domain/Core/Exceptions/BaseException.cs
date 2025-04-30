namespace Yes.Domain.Core.Exceptions
{
    public class BaseException : Exception
    {
        public BaseException(string? message) : base(message)
        {

        }
        public BaseException() : base()
        {

        }
    }
}
