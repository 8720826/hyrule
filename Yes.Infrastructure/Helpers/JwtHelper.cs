namespace Yes.Infrastructure.Helpers
{
    public class JwtHelper
    {
        public static string GenerateKey(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                input = "hyrule";
            };

            using (var sha256 = SHA256.Create())
            {
                // 使用 UTF-8 编码输入字符串
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // 计算 SHA256 哈希值
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // 将哈希结果转换为 Base64 字符串
                return Convert.ToBase64String(hashBytes);
            }
        }


        public static string GenerateToken(Claim[] claims, string secretKey, int tokenLifetimeMinutes)
        {
            var securityKeyBase64 = JwtHelper.GenerateKey(secretKey ?? "");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKeyBase64));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "hyrule",
                claims: claims,
                expires: DateTime.Now.AddMinutes(tokenLifetimeMinutes),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
