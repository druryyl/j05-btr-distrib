using btr.domain.SalesContext.CustomerAgg;

namespace btr.domain.SalesContext.RuteAgg
{
    public class RuteCustomerModel : IRuteKey, ICustomerKey
    {
        public string RuteId { get; set; }
        public int NoUrut { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string Address { get; set; }
    }
}
