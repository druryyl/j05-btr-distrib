using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.PurchaseContext.ReturBeliFeature
{
    public class ReturBeliModel : IReturBeliKey, ISupplierKey, IWarehouseKey
    {
        public string ReturBeliId { get; set; }
        public DateTime ReturBeliDate { get; set; }
        public string UserId { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }

        public decimal DiscountLain { get; set; }
        public decimal BiayaLain { get; set; }
        public List<ReturBeliItemModel> ListItem { get; set; }
    }

    public class ReturBeliItemModel : IReturBeliKey, IBrgKey
    {
        public string ReturBeliId { get; set; }
        public string ReturBeliItemId { get; set; }
        public int NoUrut { get; set; }
        public string BrgId { get; set; }
        public string BrgName { get; set; }
        public int Qty { get; set; }
        public string Satuan { get; set; }
        public decimal Harga { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiskonProsen { get; set; }
        public decimal DiskonRp { get; set; }
        public decimal TaxProsen { get; set; }
        public decimal TaxRp { get; set; }
        public decimal Total { get; set; }
    }
}
