using btr.domain.SalesContext.OrderAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace btr.application.SalesContext.OrderFeature
{
    public interface  IOrderBuilder : INunaBuilder<OrderModel>
    {
        IOrderBuilder Load(IOrderKey key);
    }
    public class OrderBuilder : IOrderBuilder
    {
        private readonly IOrderDal _orderDal;
        private readonly IOrderItemDal _orderItemDal;
        private OrderModel _agg;

        public OrderBuilder(IOrderDal orderDal, 
            IOrderItemDal orderItemDal)
        {
            _orderDal = orderDal;
            _orderItemDal = orderItemDal;
            _agg = new OrderModel();
            _agg.ListItems = new List<OrderItemType>();
        }

        public IOrderBuilder Load(IOrderKey key)
        {
            _agg = _orderDal.GetData(key);
            if (_agg == null)
                throw new ArgumentNullException(nameof(_agg), "Order not found.");
            _agg.ListItems = _orderItemDal.ListData(key)?.ToList()
                ?? new List<OrderItemType>();
            return this;
        }

        public OrderModel Build()
        {
            _agg.RemoveNull();
            return _agg;
        }
    }
}
