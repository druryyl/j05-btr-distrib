using System.Collections.Generic;

namespace btr.domain.SupportContext.ParamUser
{
    public class ParamUserModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<ParamUserItemModel> ListParam { get; set; }
    }

    public class ParamUserItemModel
    {
        public string UserId { get; set; }
        public string ParamKey { get; set; }
        public string ParamVal { get; set; }
    }
}