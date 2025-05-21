using JetBrains.Annotations;

namespace btr.distrib.PurchaseContext.ReturBeliAgg
{
    [PublicAPI]
    public class ReturBeliItemDto
    {
        public ReturBeliItemDto()
        {
        }

        public void SetPpnProsen(decimal ppnProsen)
        {
            PpnProsen = ppnProsen;
        }

        public string BrgId { get; set; }
        public string BrgCode { get; private set; }
        public string BrgName { get; private set; }
        public string HrgInputStr { get; set; }
        public string HrgDetilStr { get; private set; }

        public string QtyInputStr { get; set; }
        public string QtyDetilStr { get; private set; }

        public int QtyBesar { get; private set; }
        public string SatBesar { get; private set; }
        public int Conversion { get; private set; }
        public decimal HppSatBesar { get; private set; }

        public int QtyKecil { get; private set; }
        public string SatKecil { get; private set; }
        public decimal HppSatKecil { get; private set; }

        //      harga
        public int QtyBeli { get; private set; }
        public decimal HppSat { get; private set; }
        public decimal SubTotal { get; private set; }

        public int QtyBonus { get; private set; }
        public int QtyPotStok { get; private set; }

        //      diskon
        public string DiscInputStr { get; set; }
        public string DiscDetilStr { get; private set; }
        public decimal DiscRp { get; private set; }

        //      ppn
        public decimal DppProsen { get; set; }
        public decimal DppRp { get; private set; }
        public decimal PpnProsen { get; set; }
        public decimal PpnRp { get; private set; }

        public decimal Total { get; private set; }

    }
}
