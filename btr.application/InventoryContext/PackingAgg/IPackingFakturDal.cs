using btr.domain.InventoryContext.PackingAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.PackingAgg
{

    public interface IPackingFakturDal :
        IInsertBulk<PackingFakturModel>,
        IDelete<IPackingKey>,
        IListData<PackingFakturModel, IPackingKey>
    {
    }
}
