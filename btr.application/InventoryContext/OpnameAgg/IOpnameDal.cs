using btr.domain.InventoryContext.OpnameAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.OpnameAgg
{
    public interface IOpnameDal :
        IInsert<OpnameModel>,
        IUpdate<OpnameModel>,
        IDelete<IOpnameKey>,
        IGetData<OpnameModel, IOpnameKey>,
        IListData<OpnameModel, Periode>
    {
        
    }
}