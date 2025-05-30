﻿using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SupportContext.ParamSistemAgg;
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
        public string PeriodeDokPendukung { get; set; }
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

        public void CreateFrom(FakturModel faktur, CustomerModel customer, List<ParamSistemModel> listParam)
        {
            const string JENIS_FAKTUR = "Normal";
            const string NEGARA_PEMBELI = "IDN";
            const string KODE_TRSANSAKSI = "04";
            const string DEFAULT_ID = "0000000000000000";

            var client_npwp = listParam.FirstOrDefault(x => x.ParamCode == "CLIENT_NPWP").ParamValue;
            var id_tku_penjual = $"{client_npwp}000000";
            string npwp;
            string noDokPembeli;

            switch (customer.JenisIdentitasPajak)
            {
                case "TIN":
                    npwp = customer.Npwp;
                    noDokPembeli = customer.Npwp;
                    break;
                case "National ID":
                    npwp = DEFAULT_ID;
                    noDokPembeli = customer.Nik;
                    break;
                default:
                    npwp = DEFAULT_ID;
                    noDokPembeli = DEFAULT_ID;
                    break;
            }

            FakturId = faktur.FakturId;
            TanggalFaktur = faktur.FakturDate.Date;
            JenisFaktur = JENIS_FAKTUR;
            KodeTransaksi = KODE_TRSANSAKSI;
            KeteranganTambahan = string.Empty;
            DokumenPendukung = string.Empty;
            Referensi = faktur.FakturCode;
            CapFasilitas = string.Empty;
            IdTkuPenjual = id_tku_penjual;
            NpwpNikPembeli = npwp;
            JenisIdPembeli = customer.JenisIdentitasPajak;
            NegaraPembeli = NEGARA_PEMBELI;
            NomorDokumenPembeli = noDokPembeli;
            NamaPembeli = customer.NamaWp;
            AlamatPembeli = faktur.Address;
            EmailPembeli = customer.Email;
            IdTkuPembeli = customer.Nitku;
        }
    }
}
