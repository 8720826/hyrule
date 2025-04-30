
namespace Yes.Application.Admins.Storages
{
	public class QiniuProvider
	{
		public static string CreateToken(
			string accessKey,
			string secretKey,
			string scope
			)
		{
			Mac mac = new Mac(accessKey, secretKey);
			PutPolicy putPolicy = new PutPolicy();
			putPolicy.Scope = scope;
			putPolicy.SetExpires(60);
			putPolicy.SaveKey = "$(etag)$(ext)";

			string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

			return token;
		}
	}
}
