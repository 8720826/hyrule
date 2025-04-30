namespace Yes.Domain.Core.Exceptions
{

    public class GeneratorSlugException : BaseException
    {
        public GeneratorSlugException() : base($"缩略名自动生成失败，请手动指定！")
        {

        }
    }
}
