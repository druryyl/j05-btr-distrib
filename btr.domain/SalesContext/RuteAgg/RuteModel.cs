using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.SalesContext.RuteAgg
{
    public class RuteModel : IRuteKey
    {
        public string RuteId { get; set; }
        public string RuteCode { get; set; }
        public string RuteName { get; set; }
        public List<RuteCustomerModel> ListCustomer { get; set; }
    }
}
