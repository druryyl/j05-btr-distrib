using btr.domain.InventoryContext.OpnameAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.OpnameAgg
{
    public interface IStokOpDal :
        IInsert<StokOpModel>,
        IUpdate<StokOpModel>,
        IDelete<IStokOpKey>,
        IGetData<StokOpModel, IStokOpKey>,
        IListData<StokOpModel, Periode, IWarehouseKey>
    {
    }
}
