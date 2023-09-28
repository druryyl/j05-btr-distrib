namespace btr.distrib.SalesContext.FakturAgg
{
    public class FakturItemDto
    {
        public FakturItemDto()
        {
            PpnProsen = 11; 
        }
        public string BrgId { get; set; }
        public string BrgCode { get; private set; }
        public string BrgName { get; private set; }
        public string StokHargaStr { get; private set; }

        public string QtyInputStr { get; set; }
        public string QtyDetilStr { get; private set; }

        public int QtyBesar { get; private set; }
        public string SatBesar { get; private set; }
        public int Conversion { get; private set; }
        public decimal HrgSatBesar { get; private set; }

        public int QtyKecil { get; private set; }
        public string SatKecil { get; private set; }
        public decimal HrgSatKecil { get; private set; }

        //      harga
        public int QtyJual { get; private set; }
        public decimal HrgSat { get; private set; }
        public decimal SubTotal { get; private set; }

        public int QtyBonus { get; private set; }
        public int QtyPotStok { get; private set; }

        //      diskon
        public string DiscInputStr { get; set; }
        public string DiscDetilStr { get; private set; }
        public decimal DiscRp { get; private set; }

        //      ppn
        public decimal PpnProsen { get; set; }
        public decimal PpnRp { get; private set; }

        public decimal Total { get; private set; }
    }
}
