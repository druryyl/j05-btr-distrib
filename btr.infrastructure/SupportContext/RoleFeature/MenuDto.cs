using btr.domain.SupportContext.RoleFeature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.infrastructure.SupportContext.RoleFeature
{
    public class MenuDto
    {
        public MenuDto(string menuId, 
            int groupOrder, string formType,
            string menuName, string caption)
        {
            MenuId = menuId;
            GroupOrder = groupOrder;
            FormType = formType;
            MenuName = menuName;
            Caption = caption;
        }

        public string MenuId { get; set; }
        public int GroupOrder { get; set; }
        public string FormType { get; set; }
        public string MenuName { get; set; }
        public string Caption { get; set; }

        public MenuType ToModel()
        {
            var result = new MenuType(MenuId, GroupOrder, FormType, MenuName, Caption);
            return result;
        }

        public static MenuDto FromModel(MenuType model)
        {
            var result = new MenuDto(model.MenuId, model.GroupOrder, model.FormType, 
                model.MenuName, model.Caption);
            return result;
        }
    }
}
