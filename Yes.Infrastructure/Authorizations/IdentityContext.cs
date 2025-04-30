namespace Yes.Infrastructure.Authorizations
{
    public class IdentityContext : IIdentityContext
    {
        private readonly IHttpContextAccessor _accessor;
        public IdentityContext(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }


        public int Id
        {
            get
            {
                if (!IsAuthenticated())
                {
                    throw new AuthenticationException("请登录或刷新页面后操作！");
                }

                int.TryParse(_accessor.HttpContext!.User?.FindFirst($"Id")?.Value, out var id);
                if (id == 0)
                {
                    throw new AuthenticationException("请登录或刷新页面后操作！");
                }

                return id;
            }
        }

        public string Name
        {
            get
            {
                if (!IsAuthenticated())
                {
                    throw new AuthenticationException("请登录或刷新页面后操作！");
                }

                return _accessor.HttpContext!.User?.FindFirst($"Name")?.Value ?? "";
            }
        }


        public IdentityRoleEnum Role
        {
            get
            {
                if (!IsAuthenticated())
                {
                    throw new AuthenticationException("请登录或刷新页面后操作！");
                }

                Enum.TryParse<IdentityRoleEnum>(_accessor.HttpContext!.User?.FindFirst($"Role")?.Value, out var role);
                return role;
            }
        }



        public bool IsAuthenticated()
        {
            return _accessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext?.User.Claims;
        }
    }
}
