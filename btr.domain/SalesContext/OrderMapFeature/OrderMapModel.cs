using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.SalesContext.OrderStatusFeature
{
    public class OrderMapModel
    {
        public string OrderId { get; set; }
        public string FakturId { get; set; }
        public string FakturCode { get; set; }
        public string UserName { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
