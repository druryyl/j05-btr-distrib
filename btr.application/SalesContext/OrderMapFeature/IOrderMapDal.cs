using btr.domain.SalesContext.OrderAgg;
using btr.domain.SalesContext.OrderStatusFeature;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.OrderMapFeature
{
    public interface IOrderMapDal :
        IInsert<OrderMapModel>,
        IUpdate<OrderMapModel>,
        IDelete<IOrderKey>,
        IGetData<OrderMapModel, IOrderKey>,
        IListData<OrderMapModel, Periode>
    {
    }
}
