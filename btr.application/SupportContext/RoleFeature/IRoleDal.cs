using btr.domain.InventoryContext.PackingOrderFeature;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.RuteAgg;
using btr.domain.SupportContext.RoleFeature;
using btr.nuna.Application;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SupportContext.RoleFeature
{
    public interface IRoleRepo :
        ISaveChange<RoleType>,
        IDeleteEntity<IRoleKey>,
        ILoadEntity<RoleType, IRoleKey>
    {
        IEnumerable<RoleType> ListRole();
    }
}
