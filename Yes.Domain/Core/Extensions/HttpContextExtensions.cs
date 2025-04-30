
namespace Yes.Domain.Core.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetIpAddress(this HttpContext httpContext)
        {
            var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
            // 如果使用了代理服务器，可能需要从X-Forwarded-For头获取真实的IP地址
            if (ipAddress == null || ipAddress == "::1") // "::1" 是IPv6的localhost
            {
                var xForwardedForHeader = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (!string.IsNullOrEmpty(xForwardedForHeader))
                {
                    ipAddress = xForwardedForHeader.Split(",").FirstOrDefault()?.Trim() ?? "";
                }
            }
            return ipAddress ?? "";
        }
    }
}
