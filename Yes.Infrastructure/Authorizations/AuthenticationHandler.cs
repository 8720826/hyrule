namespace Yes.Infrastructure.Authorizations
{
    public class AuthenticationHandler : IAuthenticationHandler
    {

        private Microsoft.AspNetCore.Authentication.AuthenticationScheme _scheme;
        private HttpContext _context;
        private readonly IJwtProvider _jwtProvider;

        public AuthenticationHandler(IJwtProvider jwtProvider)
        {
            _jwtProvider = jwtProvider;
        }

        public Task InitializeAsync(Microsoft.AspNetCore.Authentication.AuthenticationScheme scheme, HttpContext context)
        {
            _scheme = scheme;
            _context = context;
            return Task.CompletedTask;
        }


        /// <summary>
        /// 鉴权验证
        /// </summary>
        /// <returns></returns>
        public async Task<AuthenticateResult> AuthenticateAsync()
        {
            string token = _context.Request.Headers.Authorization.ToString();
            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }

            if (string.IsNullOrEmpty(token))
            {
                return await Task.FromResult(AuthenticateResult.Fail("token错误，请重新登录！"));
            }

            var claimsIdentity = await _jwtProvider.ReadToken(token);

            if (claimsIdentity == null)//验证token是否正确
            {
                return await Task.FromResult(AuthenticateResult.Fail("token错误，请重新登录！"));
            }

            var schemeName = _scheme.Name;

            //鉴权成功，写入用户信息
            return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsIdentity, _scheme.Name)));
        }

        /// <summary>
        /// 未登录
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task ChallengeAsync(AuthenticationProperties? properties)
        {
            _context.Response.StatusCode = 401;
            return Task.CompletedTask;
        }




        /// <summary>
        /// 没有权限访问
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task ForbidAsync(AuthenticationProperties? properties)
        {
            _context.Response.StatusCode = 403;
            return Task.CompletedTask;

        }

    }
}
