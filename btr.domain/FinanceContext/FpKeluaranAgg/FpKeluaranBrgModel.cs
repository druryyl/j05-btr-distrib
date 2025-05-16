using btr.domain.SalesContext.FakturAgg;
using System;

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
            const string SATUAN_UKUR = "UM.0021";
            const string KODE_BRG_JASA = "000000";

            FakturId = item.FakturId;
            BarangJasa = BRG_JASA;
            KodeBarangJasa = KODE_BRG_JASA;
            NamaBarangJasa = item.BrgName;
            NamaSatuanUkur = SATUAN_UKUR;
            HargaSatuan = Math.Round(item.HrgSat,0);
            JumlahBarangJasa = item.QtyJual;
            TotalDiskon = Math.Round(item.DiscRp,0);
            Dpp = (HargaSatuan * JumlahBarangJasa) - TotalDiskon;
            DppLain = Math.Round(Dpp * 11/12,0);   
            TarifPpn = item.PpnProsen;
            Ppn = Math.Round(DppLain * TarifPpn / 100,0);
            TarifPpnBm = 0;
            PpnBm = 0;
        }
    }
}
