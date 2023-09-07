using btr.domain.BrgContext.BrgAgg;
using btr.nuna.Infrastructure;

namespace btr.application.BrgContext.BrgAgg
{
    public interface IBrgHargaDal :
        IInsertBulk<BrgHargaModel>,
        IDelete<IBrgKey>,
        IListData<BrgHargaModel, IBrgKey>
    {
    }
}
