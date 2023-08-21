using btr.domain.BrgContext.BrgAgg;
using btr.domain.BrgContext.BrgStokViewAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.BrgContext.BrgStokViewAgg.Contracts
{
    public interface IBrgStokViewDal :
        IListData<BrgStokViewModel, IWarehouseKey>
    {
        BrgStokViewModel GetData<T>(T key) where T : IBrgKey, IWarehouseKey;
    }
}
