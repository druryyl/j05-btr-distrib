using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.SalesContext.RuteAgg
{
    public class RuteModel
    {
        public string RuteId { get; set; }
        public string RuteCode { get; set; }
        public string RuteName { get; set; }
        public List<RuteCustomerModel> ListCustomer { get; set; }
    }

    public class RuteCustomerModel
    {
        public string RuteId { get; set; }
        public int NoUrut { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string Address { get; set; }
    }
}
