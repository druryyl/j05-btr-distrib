using btr.domain.BrgContext.BrgAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.domain.SalesContext.FakturAgg;

namespace btr.distrib.InventoryContext.PackingAgg
{
    internal class Packing2BrgDto : IFakturKey, ISupplierKey, IBrgKey
    {
        public Packing2BrgDto(string fakturId, 
            string supplierId,  string brgId, string brgName, string code,
            int qtyBesar, string satBesar, int qtyKecil, string satKecil,
            decimal hargaJual)
        {
            FakturId = fakturId;
            SupplierId = supplierId;
            BrgId = brgId;
            BrgName = brgName;
            BrgCode = code;
            QtyBesar = qtyBesar;
            SatBesar = satBesar;
            QtyKecil = qtyKecil;
            SatKecil = satKecil;
            HargaJual = hargaJual;
        }

        public string FakturId { get; set; }
        public string SupplierId { get; set; }
        public string BrgId { get; set; }
        public string BrgCode { get; private set; }
        public string BrgName { get; private set; }
        public int QtyBesar { get; private set; }
        public string SatBesar { get; private set; }
        public int QtyKecil { get; private set; }
        public string SatKecil { get; private set; }
        public decimal HargaJual { get; private set; }
    }
}