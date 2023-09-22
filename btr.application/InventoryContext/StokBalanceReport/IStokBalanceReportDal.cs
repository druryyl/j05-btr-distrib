using btr.domain.InventoryContext.StokBalanceReport;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.StokBalanceReport
{
    public interface IStokBalanceReportDal :
        IListData<StokBalanceView>
    {
    }
}
