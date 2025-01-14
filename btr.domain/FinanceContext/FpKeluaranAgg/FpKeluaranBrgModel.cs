using btr.domain.SalesContext.FakturAgg;

namespace btr.domain.FinanceContext.FpKeluaranAgg
{
    public class FpKeluaranBrgModel
    {
        public string FpKeluaranBrgId { get; set; }
        public string FpKeluaranFakturId { get; set; }
        public string FpKeluaranId { get; set; }
        public string FakturId { get; set; }

        //  CONTENT
        public int Baris { get; set; }
        public string BarangJasa { get; set; }
        public string KodeBarangJasa { get; set; }
        public string NamaBarangJasa { get; set; }
        public string NamaSatuanUkur { get; set; }
        public decimal HargaSatuan { get; set; }
        public int JumlahBarangJasa { get; set; }
        public decimal TotalDiskon { get; set; }
        public decimal Dpp { get; set; }
        public decimal DppLain { get; set; }
        public decimal TarifPpn { get; set; }
        public decimal Ppn { get; set; }
        public decimal TarifPpnBm { get; set; }
        public decimal PpnBm { get; set; }

        public void CreateFrom(FakturItemModel item)
        {
            const string BRG_JASA = "A";
            const string SATUAN_UKUR = "UM.0018";

            FakturId = item.FakturId;
            BarangJasa = BRG_JASA;
            KodeBarangJasa = item.BrgCode;
            NamaBarangJasa = item.BrgName;
            NamaSatuanUkur = SATUAN_UKUR;
            HargaSatuan = item.HrgSat;
            JumlahBarangJasa = item.QtyJual;
            TotalDiskon = item.DiscRp;
            Dpp = item.Total;
            DppLain = item.DppRp;
            TarifPpn = item.PpnProsen;
            Ppn = item.PpnRp;
            TarifPpnBm = 0;
            PpnBm = 0;
        }
    }
}
