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
            int conversion, int qty, double hargaJual)
        {
            NoUrut = qtyHargaNo;
            BrgId = brgId;
            Satuan = satuan;
            Conversion = conversion;
            Qty = qty;
            HargaJual = hargaJual;
        }
        public string FakturId { get; set; }
        public string FakturItemId { get; set; }
        public string FakturQtyHargaId { get; set; }
        public int NoUrut { get; set; }
    
        public string BrgId { get; set; }
        public string Satuan { get; set; }
        public int Conversion { get; set; } 
        public int Qty { get; set; }
        public double HargaJual { get; set; }
    }
}

