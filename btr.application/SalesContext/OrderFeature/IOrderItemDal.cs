using btr.domain.SalesContext.OrderAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.OrderFeature
{
    public interface IOrderItemDal :
        IInsertBulk<OrderItemType>,
        IDelete<IOrderKey>,
        IListData<OrderItemType, IOrderKey>
    {
    }
}
