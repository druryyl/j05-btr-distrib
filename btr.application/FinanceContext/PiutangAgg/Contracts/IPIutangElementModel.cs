using btr.domain.FinanceContext.PiutangAgg;
using btr.nuna.Infrastructure;

namespace btr.application.FinanceContext.PiutangAgg.Contracts
{
    public interface IPIutangElementModel :
        IInsertBulk<PiutangElementModel>,
        IDelete<IPiutangKey>,
        IListData<PiutangElementModel, IPiutangKey>
    {
    }
}