using System.Collections.Generic;
using btr.domain.BrgContext.BrgAgg;

namespace btr.domain.SalesContext.FakturAgg
{
    public class FakturItemModel : IFakturKey, IFakturItemKey, IBrgKey
    {
        public string FakturId { get; set; }
        public string FakturItemId { get; set; }
        public int NoUrut { get; set; }

        //  info barang
        public string BrgId { get; set; }
        public string BrgName { get; set; }
        public string BrgCode { get; set; }
        public string StokHargaStr { get; set; }
        
        public string QtyInputStr { get; set; }
        public string QtyDetilStr { get; set; }

        public int QtyBesar { get; set; }
        public string SatBesar { get; set; }
        public int Conversion { get; set; }
        public decimal HrgSatBesar { get; set; }

        public int QtyKecil { get; set; }
        public string SatKecil { get; set; }
        public decimal HrgSatKecil { get; set; }

        //  harga
        public int QtyJual { get; set; }
        public decimal HrgSat { get; set; }
        public decimal SubTotal { get; set; }

        public int QtyBonus { get; set; }
        public int QtyPotStok { get; set; }


        //  diskon
        public string DiscInputStr { get; set; }
        public string DiscDetilStr { get; set; }
        public decimal DiscRp { get; set; }
        
        //  ppn
        public decimal PpnProsen { get; set; }
        public decimal PpnRp { get; set; }
        
        public decimal Total { get; set; }
    
        public List<FakturDiscountModel> ListDiscount { get; set; }
    }
}