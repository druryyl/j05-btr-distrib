using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.SupportContext.RoleFeature
{
    public class RoleType : IRoleKey
    {
        private readonly List<MenuType> _listMenu;
        public RoleType(string roleId, string roleName, IEnumerable<MenuType> listMenu)
        {
            RoleId = roleId;
            RoleName = roleName;
            _listMenu = listMenu?.ToList() ?? new List<MenuType>();
        }
        public string RoleId { get; private set; }
        public string RoleName { get; private set; }
        public IEnumerable<MenuType> ListMenu => _listMenu;

        public static RoleType Default => new RoleType("-", "-", Enumerable.Empty<MenuType>());
        public static IRoleKey Key(string id)
        {
            return new RoleType(id, "-", Enumerable.Empty<MenuType>());
        }
        public static RoleType Create(string roleId, string roleName)
        {
            var result = new RoleType(roleId, roleName, Enumerable.Empty<MenuType>());
            return result;
        }

        public void AddMenu(MenuType menu)
        {
            _listMenu.RemoveAll(x => x.MenuId == menu.MenuId);
            _listMenu.Add(menu);
        }
        public void RemoveMenu(MenuType menu)
        {
            _listMenu.RemoveAll(x => x.MenuId == menu.MenuId);
        }

        public void ClearListMenu()
        {
            _listMenu.Clear();
        }
    }

    public interface IRoleKey
    {
        string RoleId { get; }
    }
}
