using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.SupportContext.RoleFeature
{
    public class MenuType : IMenuKey
    {
        public MenuType(string menuId, 
            int groupOrder, string formType,
            string menuName, string caption)
        {
            MenuId = menuId;
            GroupOrder = groupOrder;
            FormType = formType;
            MenuName = menuName;
            Caption = caption;
        }

        public string MenuId { get; private set; }
        public int GroupOrder { get; private set; }
        public string FormType { get; private set; }
        public string MenuName { get; private set; }
        public string Caption { get; private set; }

        public static MenuType Default => new MenuType("-", 0, "-", "-", "-");

        public static IMenuKey Key(string id)
        {
            return new MenuType(id, 0, "-", "-", "-");
        }
    }

    public interface IMenuKey
    {
        string MenuId { get; }
    }
}
