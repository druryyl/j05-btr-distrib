using btr.domain.BrgContext.BrgAgg;

namespace btr.domain.InventoryContext.ReturJualAgg
{
    public class ReturJualItemModel : IReturJualKey, IBrgKey
    {
        public string ReturJualId { get; set; }
        public int NoUrut { get; set; }
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        
        public string QtyInputStr { get; set; }
        public int QtyBesar { get; set; }
        public string SatBesar { get; set; }
        public decimal HrgSatBesar { get; set; }
        public int Conversion { get; set; }
        
        public int  QtyKecil { get; set; }
        public string SatKecil { get; set; }
        public decimal HrgSatKecil { get; set; }
        
        public int Qty { get; set; }
        public decimal HrgSat { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscRp { get; set; }
        public decimal PpnRp { get; set; }
        public decimal Total { get; set; }
        
    }
}