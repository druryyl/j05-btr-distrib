using btr.domain.InventoryContext.PackingOrderFeature;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.PackingOrderFeature
{
    public interface IPackingOrderRepo :
        ISaveChange<PackingOrderModel>,
        IDeleteEntity<IPackingOrderKey>,
        ILoadEntity<PackingOrderModel, IPackingOrderKey>,
        ILoadEntity<PackingOrderModel, IFakturKey>
    {
    }
}
