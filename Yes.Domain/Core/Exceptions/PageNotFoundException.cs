namespace Yes.Domain.Core.Exceptions
{

    public class PageNotFoundException : BaseException
    {
        public PageNotFoundException(string url) : base($"未找到您访问的页面 {url}！")
        {

        }

        public PageNotFoundException() : base($"未找到您访问的页面！")
        {

        }
    }
}
