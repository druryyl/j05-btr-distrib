namespace btr.domain.SupportContext.UserAgg
{
    public class UserModel : IUserKey
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}