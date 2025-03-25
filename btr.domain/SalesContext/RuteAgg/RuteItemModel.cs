using btr.domain.SalesContext.CustomerAgg;

namespace btr.domain.SalesContext.RuteAgg
{
    public class RuteItemModel : IRuteKey, ICustomerKey
    {
        public RuteItemModel()
        {
        }
        public RuteItemModel(string ruteId, int noUrut, string customerId, string customerCode, string address)
        {
            RuteId = ruteId;
            NoUrut = noUrut;
            CustomerId = customerId;
            CustomerCode = customerCode;
            Address = address;

        }
        public string RuteId { get; set; }
        public int NoUrut { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string Address { get; set; }
    }
}
