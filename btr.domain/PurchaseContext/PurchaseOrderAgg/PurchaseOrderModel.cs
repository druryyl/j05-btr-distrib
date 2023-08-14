using btr.domain.BrgContext.BrgAgg;
using btr.domain.PurchaseContext.SupplierAgg;

namespace btr.domain.PurchaseContext.PurchaseOrderAgg
{
    public class PurchaseOrderModel : IPurchaseOrderKey, ISupplierKey
    {
        public string PurchaseOrderId { get; set; }
        public string PurchaseOrderDate { get; set; }
        public string UserId { get; set; }
        public string SupplierId { get; set; }
        public string SupplierNameName { get; set; }
    }

    public class PurchaseOrderItemModel : IPurchaseOrderKey, IBrgKey
    {
        public string PurchaseOrderId { get; set; }
        public string PurchaseOrderItemId { get; set; }
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