using System.Collections.Generic;
using btr.domain.BrgContext.BrgAgg;

namespace btr.domain.SalesContext.FakturAgg
{
    public class FakturItemModel : IFakturKey, IFakturItemKey, IBrgKey
    {
        public string FakturId { get; set; }
        public string FakturItemId { get; set; }
        public int NoUrut { get; set; }

        public string BrgId { get; set; }
        public string BrgName { get; set; }
    
        public int AvailableQty { get; set; }
        public int Qty { get; set; }
        public decimal HargaJual { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountRp { get; set; }
        public decimal PpnProsen { get; set; }
        public decimal PpnRp { get; set; }
        public decimal Total { get; set; }
    
        public List<FakturQtyHargaModel> ListQtyHarga { get; set; }
        public List<FakturDiscountModel> ListDiscount { get; set; }
    }
}