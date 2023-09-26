using btr.domain.InventoryContext.StokBalanceReport;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.StokBalanceReport
{
    public interface IStokBalanceReportDal :
        IListData<StokBalanceView>
    {
    }
}
