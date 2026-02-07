using btr.domain.BrgContext.BrgAgg;
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
        IInsert<DepoType>,
        IUpdate<DepoType>,
        IDelete<IDepoKey>,
        IGetData<DepoType, IDepoKey>,
        IGetData<DepoType, IBrgKey>,
        IListData<DepoType>
    {
    }
}
