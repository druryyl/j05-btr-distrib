using btr.domain.InventoryContext.DriverAgg;
using btr.nuna.Infrastructure;

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
