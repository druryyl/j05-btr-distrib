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
        
        //  qty
        public string QtyInputStr { get; set; }
        public string QtyDetilStr { get; set; }
        public int QtyPotStok { get; set; }     //  qty-termasuk-bonus (utk potong stok)
        public int QtyJual { get; set; }        //  qty-tanpa-bonys (utk hitung harga)

        //  harga
        public decimal HargaSatuan { get; set; }
        public decimal SubTotal { get; set; }

        //  diskon
        public string DiscInputStr { get; set; }
        public string DiscDetilStr { get; set; }
        public decimal DiscRp { get; set; }
        
        //  ppn
        public decimal PpnProsen { get; set; }
        public decimal PpnRp { get; set; }
        
        public decimal Total { get; set; }
    
        public List<FakturQtyHargaModel> ListQtyHarga { get; set; }
        public List<FakturDiscountModel> ListDiscount { get; set; }
    }
}