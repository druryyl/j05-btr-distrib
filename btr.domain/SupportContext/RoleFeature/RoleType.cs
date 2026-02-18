using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.SupportContext.RoleFeature
{
    public class RoleType : IRoleKey
    {
        public RoleType(string roleId, string roleName)
        {
            RoleId = roleId;
            RoleName = roleName;
        }
        public string RoleId { get; private set; }
        public string RoleName { get; private set; }

        public static RoleType Default => new RoleType("-", "-");
        public static IRoleKey Key(string id)
        {
            return new RoleType(id, "-");
        }
    }

    public interface IRoleKey
    {
        string RoleId { get; }
    }
}
