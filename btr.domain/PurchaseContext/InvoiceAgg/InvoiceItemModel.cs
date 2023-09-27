using System.Collections.Generic;
using btr.domain.BrgContext.BrgAgg;

namespace btr.domain.PurchaseContext.InvoiceAgg
{
    public class InvoiceItemModel : IInvoiceKey, IBrgKey
    {
        public string InvoiceId { get; set; }
        public string InvoidItemId { get; set; }
        public int NoUrut { get; set; }

        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        
        public string QtyInputStr { get; set; }
        public string QtyDetilStr { get; set; }
        public int QtyBesar { get; set; }
        public string SatBesar { get; set; }
        public int Conversion { get; set; }
        public decimal HrgSatBesar { get; set; }

        public int QtyKecil { get; set; }
        public string SatKecil { get; set; }
        public decimal HrgSatKecil { get; set; }

        public int QtyJual { get; set; }
        public decimal HrgSat { get; set; }
        public decimal SubTotal { get; set; }

        public int QtyBonus { get; set; }
        public int QtyPotStok { get; set; }

        public string DiscInputStr { get; set; }
        public string DiscDetilStr { get; set; }
        public decimal DiscRp { get; set; }

        public decimal PpnProsen { get; set; }
        public decimal PpnRp { get; set; }
        public decimal Total { get; set; }
        
        public List<InvoiceDiscModel> ListDisc { get; set; }
    }
}