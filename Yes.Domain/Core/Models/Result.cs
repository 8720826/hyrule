namespace Yes.Domain.Core.Models
{
    public interface IResult
    {
        bool Success { get; set; }

        string Message { get; set; }
    }

    public interface IResult<T> : IResult
    {

        T Data { get; set; }
    }

    public class Result<T> : IResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public T Data { get; set; }

        public static Result<T> Ok()
        {
            return new Result<T>()
            {
                Success = true
            };
        }


        public static Result<T> Ok(T data, string message = "")
        {
            return new Result<T>()
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static Result<T> Error(string message = "")
        {
            return new Result<T>()
            {
                Success = false,
                Message = message
            };
        }

        public static Result<T> Error(T data, string message = "")
        {
            return new Result<T>()
            {
                Success = false,
                Message = message,
                Data = data
            };
        }
    }


    public class Result : Result<object>
    {

    }
}
