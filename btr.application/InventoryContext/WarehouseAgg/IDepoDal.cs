using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.WarehouseAgg
{
    public interface IDepoDal :
        IInsert<DepoModel>,
        IUpdate<DepoModel>,
        IDelete<IDepoKey>,
        IGetData<DepoModel, IDepoKey>,
        IListData<DepoModel>
    {
    }
}
