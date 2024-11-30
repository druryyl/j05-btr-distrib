using System;
using JetBrains.Annotations;

namespace btr.distrib.SalesContext.FakturControlAgg
{
    [PublicAPI]
    public class FakturControlView
    {
        public string FakturId { get; private set; }
        public string FakturCode { get; private set; }
        public DateTime FakturDate { get; private set; }
        public string CustomerName { get; private set; }
        public string Npwp { get; private set; }
        public string SalesPersonName { get; private set; }
        public decimal GrandTotal { get; private set; }
        public decimal Bayar { get; private set; }
        public decimal Sisa { get; private set; }

        public bool Posted { get; set; }
        public bool Kirim { get; set; }
        public bool Kembali { get; set; }
        public bool Lunas { get; private set; }
        public bool Pajak { get; private set; }
        public string NoFakturPajak { get; private set; }
        public string UserId { get; private set; }

        public void SetLunas(bool val) => Lunas = val;
        public void SetPajak(bool val) => Pajak = val;
        public void SetNilai(decimal grandTotal, decimal bayar)
        {
            GrandTotal = grandTotal;
            Bayar = bayar;
            Sisa = grandTotal - bayar;
        }
        
        public void FormatFakturCode()=> FakturCode = $"{FakturCode.Substring(0, 1)}-{FakturCode.Substring(1, 3)}-{FakturCode.Substring(4, 4)}";
    }
}