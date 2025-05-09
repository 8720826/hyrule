namespace Yes.Infrastructure.Middlewares
{
    public class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var title = "";
            var message = "";
            var exType = ex.GetType();

            if (IsCustomException(exType))
            {
                title = "系统错误";
                message = ex.Message;
                _logger.LogError($"{exType.Name}：{ex.Message}");
            }
            else if (exType == typeof(AuthenticationException))
            {
                title = "认证失败";
                message = "请登录或刷新页面后操作！";
                _logger.LogError(ex, "Exception");
            }
            else if (exType == typeof(SqlException))
            {
                title = "数据操作异常";
                message = "读取或写入数据异常。";
                _logger.LogError(ex, "Exception");
            }
            else
            {
                title = "服务器内部错误";
                message = ex.Message;
                _logger.LogError(ex, "Exception");
            }

            var acceptHeader = context.Request.Headers["Accept"].FirstOrDefault();
            var isAjaxRequest = acceptHeader?.Contains("application/json") == true;
            if (isAjaxRequest)
            {
                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = title,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Detail = message
                };
                await context.Response.WriteAsJsonAsync(problem);
            }
            else
            {
                await context.Response.WriteAsync(message);
            }

        }

        private static bool IsCustomException(Type type)
        {
            return typeof(BaseException).IsAssignableFrom(type);
        }



    }
}
