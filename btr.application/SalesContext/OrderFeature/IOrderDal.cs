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

    public interface IOrderSummaryDal
    {
        IEnumerable<OrderSummaryDto> ListDataSummary(Periode periode);
    }

    public class OrderSummaryDto
    {
        public OrderSummaryDto(string orderDate, string sales, int outlet, 
            int order, int item, decimal total)
        {
            OrderDate = orderDate;
            Sales = sales;
            Outlet = outlet;
            Order = order;
            Item = item;
            Total = total;
        }
        public string OrderDate { get; private set; }
        public string Sales { get; private set; }
        public int Outlet { get; private set; }
        public int Order { get; private set; }
        public int Item { get; private set; }
        public decimal Total { get; private set; }
    }

}
