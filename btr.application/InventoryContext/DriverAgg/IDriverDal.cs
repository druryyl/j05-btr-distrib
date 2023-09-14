using btr.domain.InventoryContext.DriverAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.DriverAgg
{
    public interface IDriverDal :
        IInsert<DriverModel>,
        IUpdate<DriverModel>,
        IDelete<IDriverKey>,
        IGetData<DriverModel, IDriverKey>,
        IListData<DriverModel>
    {
    }
}
