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
        public OrderSummaryDto(string orderDate, string userEmail, int orderCount, int orderItemCount, decimal totalAmountSum)
        {
            OrderDate = orderDate;
            UserEmail = userEmail;
            OrderCount = orderCount;
            OrderItemCount = orderItemCount;
            TotalAmountSum = totalAmountSum;
        }
        public string OrderDate { get; private set; }
        public string UserEmail { get; private set; }
        public int OrderCount { get; private set; }
        public int OrderItemCount { get; private set; }
        public decimal TotalAmountSum { get; private set; }
    }

}
