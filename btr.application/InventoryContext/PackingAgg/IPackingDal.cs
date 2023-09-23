using btr.domain.InventoryContext.PackingAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.PackingAgg
{
    public interface IPackingDal :
        IInsert<PackingModel>,
        IUpdate<PackingModel>,
        IDelete<IPackingKey>,
        IGetData<PackingModel, IPackingKey>,
        IListData<PackingModel, Periode>
    {
    }
}
