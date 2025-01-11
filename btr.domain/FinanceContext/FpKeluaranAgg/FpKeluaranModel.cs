using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.FinanceContext.FpKeluaranAgg
{
    public class FpKeluaranModel
    {
        public string FpKeluaranId { get; set; }
        public DateTime FpKeluaranDate { get; set; }
        public string UserId { get; set; }

        public List<FpKeluaranFakturModel> ListFaktur { get; set; }
    }


    public class FpKeluaranFakturModel
    {
        public string FpKeluaranFakturId { get; set; }
        public string FpKeluaranId { get; set; }
        public string FakturId { get; set; }

        //  CONTENT
        public int Baris { get; set; }
        public DateTime TanggalFaktur { get; set; }
        public string JenisFaktur { get; set; }
        public string KodeTransaksi { get; set; }
        public string KeteranganTambahan { get; set; }
        public string DokumenPendukung { get; set; }
        public string Referensi { get; set; }
        public string CapFasilitas { get; set; }
        public string IdTkuPenjual { get; set; }
        public string NpwpNikPembeli { get; set; }
        public string JenisIdPembeli { get; set; }

        public string NegaraPembeli { get; set; }
        public string NomorDokumenPembeli { get; set; }
        public string NamaPembeli { get; set; }
        public string AlamatPembeli { get; set; }
        public string EmailPembeli { get; set; }
        public string IdTkuPembeli { get; set; }

        public List<FpKeluaranBrgModel> ListBrg { get; set; }
    }

    public class FpKeluaranBrgModel
    {
        public string FpKeluaranBrgModelId { get; set; }
        public string FpKeluaranFakturId { get; set; }
        public string FpKeluaranId { get; set; }
        public string FakturId { get; set; }

        //  CONTENT
        public int Baris { get; set; }
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
    }
}
