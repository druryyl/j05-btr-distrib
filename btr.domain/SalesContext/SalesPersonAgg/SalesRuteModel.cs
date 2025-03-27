using btr.domain.SalesContext.CustomerAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.SalesContext.SalesPersonAgg
{
    public class SalesRuteModel : ISalesRuteKey, ISalesPersonKey
    {
        public string SalesRuteId { get; set; }
        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
        public string HariId { get; set; }
        public List<SalesRuteItemModel> ListCustomer { get; set; }
    }

    public class SalesRuteItemModel : ISalesRuteKey, ICustomerKey
    {
        public string SalesRuteId { get; set; }
        public int NoUrut { get; set; }
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
    }
}
