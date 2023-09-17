using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.SalesContext.FakturAgg
{
    public class FakturPackingView
    {
        public string FakturId { get; set; }
        public DateTime FakturDate { get; set; }
        public string FakturCode { get; set; }

        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }

        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string Address { get; set; }
        public string Kota { get; set; }


        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public decimal GrandTotal { get; set; }

        public string PackingId { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public DateTime DeliveryDate { get; set; }

    }
}
