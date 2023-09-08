using btr.domain.BrgContext.BrgAgg;

namespace btr.domain.SalesContext.FakturAgg
{
    public class FakturQtyHargaModel : IFakturKey, 
        IFakturItemKey, IFakturStokHargaKey, IBrgKey
    {
        public FakturQtyHargaModel()
        {
        }

        public FakturQtyHargaModel(int qtyHargaNo, string brgId, string satuan, 
            decimal hargaSatuan, int conversion, int qty, decimal subTotal)
        {
            NoUrut = qtyHargaNo;
            BrgId = brgId;
            Satuan = satuan;
            HargaSatuan = hargaSatuan;
            Conversion = conversion;
            Qty = qty;
            SubTotal = subTotal;
        }
        public string FakturId { get; set; }
        public string FakturItemId { get; set; }
        public string FakturQtyHargaId { get; set; }
        public int NoUrut { get; set; }
    
        public string BrgId { get; set; }
        public string Satuan { get; set; }
        public decimal HargaSatuan { get; set; }
        public int Conversion { get; set; } 
        public int Qty { get; set; }
        public decimal SubTotal { get; set; }
    }
}

