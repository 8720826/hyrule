namespace Yes.Domain.Core.Extensions
{
    public static class PasswordExtensions
    {
        public static string ToMd5(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
        }

    }
}
