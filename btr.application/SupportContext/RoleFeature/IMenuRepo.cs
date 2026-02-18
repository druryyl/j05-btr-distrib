using btr.domain.SupportContext.RoleFeature;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SupportContext.RoleFeature
{
    public interface IMenuRepo :
        ISaveChange<MenuType>,
        IDeleteEntity<IMenuKey>,
        ILoadEntity<MenuType, IMenuKey>
    {
        IEnumerable<MenuType> ListData();
    }
}
