using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.StokPeriodikInfo
{
    public interface IStokPeriodikDal :
        IListData<StokPeriodikDto, DateTime>,
        IListData<StokPeriodikDto, DateTime, IWarehouseKey>
    {
    }

}
