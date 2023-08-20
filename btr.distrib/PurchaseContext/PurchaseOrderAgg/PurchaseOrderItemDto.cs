using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.PurchaseContext.PurchaseOrderAgg
{
    public class PurchaseOrderItemDto
    {
        public string BrgId { get; set; }
        public string BrgName { get; private set; }
        public int Qty { get; set; }
        public string Satuan { get; private set; }
        public decimal Harga { get; set; }
        public decimal SubTotal { get; private set; }
        public decimal Disc { get; set; }
        public decimal DiscRp { get; private set; }
        public decimal Tax { get; set; }
        public decimal TaxRp { get; private set; }
        public decimal Total { get; private set; }
    }
}
