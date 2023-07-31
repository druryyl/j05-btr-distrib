using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.SalesContext.SalesPersonAgg
{
    public class SalesPersonModel : ISalesPersonKey
    {
        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
    }
}