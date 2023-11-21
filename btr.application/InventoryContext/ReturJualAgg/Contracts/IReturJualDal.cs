using btr.domain.InventoryContext.ReturJualAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.ReturJualAgg.Contracts
{
    public interface IReturJualDal :
        IInsert<ReturJualModel>,
        IUpdate<ReturJualModel>,
        IDelete<IReturJualKey>,
        IGetData<ReturJualModel, IReturJualKey>,
        IListData<ReturJualModel, Periode>
    {
    }
}