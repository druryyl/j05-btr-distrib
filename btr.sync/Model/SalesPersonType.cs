using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace j07_btrade_sync.Model
{
    public class SalesPersonType
    {
        public SalesPersonType(string salesPersonId, string salesPersonCode, string salesPersonName, string serverId)
        {
            SalesPersonId = salesPersonId;
            SalesPersonCode = salesPersonCode;
            SalesPersonName = salesPersonName;
            ServerId = serverId;
        }
        public string SalesPersonId { get; set; }
        public string SalesPersonCode { get; set; }
        public string SalesPersonName { get; set; }
        public string ServerId { get; set; }
    }
}
