using btr.domain.InventoryContext.ImportOpnameAgg;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.ImportOpnameAgg.Contracts
{
    public interface IImportOpnameDal :
        IInsertBulk<ImportOpnameModel>,
        IListData<ImportOpnameModel>
    {
        void Delete();
    }
}
