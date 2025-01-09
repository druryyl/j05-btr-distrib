using btr.nuna.Application;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.StokAgg
{
    public interface IStokBalanceHealthDal :
        IListData<StokBalanceHealthDto>
    {
        void RepairStokHealth();
    }

    public class StokBalanceHealthDto
    {
        public int StokBalanceCount { get; set; }
        public int StokBalanceFailed { get; set; }
    }
}
