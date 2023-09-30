using btr.domain.InventoryContext.StokBalanceRpt;
using btr.nuna.Infrastructure;

namespace btr.application.InventoryContext.StokBalanceRpt
{
    public interface IStokBalanceReportDal :
        IListData<StokBalanceView>
    {
    }
}
