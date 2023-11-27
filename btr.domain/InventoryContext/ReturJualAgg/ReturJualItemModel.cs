using btr.domain.BrgContext.BrgAgg;
using System.Collections.Generic;

namespace btr.domain.InventoryContext.ReturJualAgg
{
    public class ReturJualItemModel : IReturJualKey, IBrgKey
    {
        public string ReturJualId { get; set; }
        public string ReturJualItemId { get; set; }
        public int NoUrut { get; set; }
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        
        //  input
        public string QtyInputStr { get; set; }
        public string HrgInputStr { get; set; }
        public string QtyHrgDetilStr { get; set; }
        public string DiscInputStr { get; set; }

        //  detil tampilan
        public string DiscDetilStr { get; set; }
        
        //  in-pcs (satuan kecil)
        public int Qty { get; set; }
        public decimal HrgSat { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscRp { get; set; }
        public decimal PpnRp { get; set; }
        public decimal Total { get; set; }

        public List<ReturJualItemQtyHrgModel> ListQtyHrg { get; set; }
        public List<ReturJualItemDiscModel> ListDisc { get; set; }
    }
}