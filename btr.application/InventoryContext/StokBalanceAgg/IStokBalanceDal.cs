using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokBalanceAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.StokBalanceAgg
{
    public interface IStokBalanceWarehouseDal  :
        IInsertBulk<StokBalanceWarehouseModel>,
        IDelete<IBrgKey>,
        IListData<StokBalanceWarehouseModel, IBrgKey>
    {
    }
}
