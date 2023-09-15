using btr.domain.InventoryContext.PackingAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.PackingAgg
{
    public interface IPackingBrgDal :
        IInsertBulk<PackingBrgModel>,
        IDelete<IPackingKey>,
        IListData<PackingBrgModel, IPackingKey>
    {
    }
}
