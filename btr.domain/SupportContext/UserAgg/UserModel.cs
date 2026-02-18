namespace btr.domain.SupportContext.UserAgg
{
    public class UserModel : IUserKey
    {
        public UserModel()
        {
        }
        public UserModel(string id) => UserId = id;

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Prefix { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}