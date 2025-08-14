using btr.domain.SalesContext.OrderAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.OrderFeature
{
    public interface IOrderDal :
        IInsert<OrderModel>,
        IUpdate<OrderModel>,
        IDelete<IOrderKey>,
        IGetData<OrderModel, IOrderKey>,
        IListData<OrderModel, Periode>
    {
    }
}
