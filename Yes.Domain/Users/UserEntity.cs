namespace Yes.Domain.Users
{

    /// <summary>
    /// 用户表
    /// </summary>
    [Table("User")]
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }


        /// <summary>
        /// 用户名
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(50)]
        public string Email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(32)]
        public string Password { get; set; }

        /// <summary>
        /// 系统用户
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(20)]
        public string NickName { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? RegDate { get; set; }

        /// <summary>
        /// 个人网站
        /// </summary>
        [StringLength(50)]
        public string Url { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserEnum Status { get; set; }



        public static UserEntity Create(string name,
            string email,
            string nickName,
            string password,
            bool isSystem = false 
            )
        {

            return new UserEntity
            {
                Email = email,
                Name = name,
                Password = password.ToMd5(),
                RegDate = DateTime.Now,
                NickName = nickName,
                Url = "",
                IsSystem = isSystem,
                Status = UserEnum.Normal
            };
        }

        public void Update(string name,
            string email,
            string nickName,
            string password)
        {
            Email = email;
            Name = name;
            NickName = nickName;

            if (!string.IsNullOrEmpty(password))
            {
                UpdatePassword(password);
            }
        }

        public void UpdateProfile(string email,
            string nickName,string url)
        {
            Email = email;
            Url = url;
            NickName = nickName;
        }

        public bool IsSystemUser()
        {
            return IsSystem;
        }

        public void CheckPassword(string password)
        {
            if (Password != password.ToMd5())
            {
                throw new InvalidPasswordException();
            }
        }

        public void UpdatePassword(string password)
        {
            Password = password.ToMd5();
        }

    }
}
