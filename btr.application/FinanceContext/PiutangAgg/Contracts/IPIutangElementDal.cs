using btr.domain.FinanceContext.PiutangAgg;
using btr.nuna.Infrastructure;

namespace btr.application.FinanceContext.PiutangAgg.Contracts
{
    public interface IPiutangElementDal :
        IInsertBulk<PiutangElementModel>,
        IDelete<IPiutangKey>,
        IListData<PiutangElementModel, IPiutangKey>
    {
    }
}