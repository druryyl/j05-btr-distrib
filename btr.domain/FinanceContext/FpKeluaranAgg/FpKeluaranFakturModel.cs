using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using System;
using System.Collections.Generic;
using System.Linq;

namespace btr.domain.FinanceContext.FpKeluaranAgg
{
    public class FpKeluaranFakturModel : IFakturKey
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

        public decimal Ppn { get; set; }
        public void CalculatePpn()
        {
            Ppn = ListBrg.Sum(x => x.Ppn);
        }
        public List<FpKeluaranBrgModel> ListBrg { get; set; }

        public void CreateFrom(FakturModel faktur, CustomerModel customer)
        {
            const string JENIS_FAKTUR = "Normal";
            //const string ID_PEMBELI_TIN = "TIN";
            //const string ID_PEMBELI_OTHER = "Other ID";
            const string NEGARA_PEMBELI = "IDN";
            const string KODE_TRSANSAKSI = "04";
            const string ID_TKU_PENJUAL = "0128872306524000000000";
            //const string NOMOR_DOKUMEN_PEMBELI_OTHER = "0000000000000000";

            //var isNpwp = customer.Npwp != NOMOR_DOKUMEN_PEMBELI_OTHER;

            FakturId = faktur.FakturId;
            TanggalFaktur = faktur.FakturDate.Date;
            JenisFaktur = JENIS_FAKTUR;
            KodeTransaksi = KODE_TRSANSAKSI;
            KeteranganTambahan = string.Empty;
            DokumenPendukung = string.Empty;
            Referensi = faktur.FakturCode;
            CapFasilitas = string.Empty;
            IdTkuPenjual = ID_TKU_PENJUAL;
            NpwpNikPembeli = customer.Npwp;
            JenisIdPembeli = customer.JenisIdentitasPajak;
            NegaraPembeli = NEGARA_PEMBELI;
            NomorDokumenPembeli = customer.Npwp;
            NamaPembeli = faktur.CustomerName;
            AlamatPembeli = faktur.Address;
            EmailPembeli = customer.Email;
            IdTkuPembeli = customer.Nitku;
        }
    }
}
