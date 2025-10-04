using JetBrains.Annotations;
using System;

namespace btr.distrib.FinanceContext.TagihanAgg
{
    [PublicAPI]
    public class TagihanFakturDto
    {
        public string FakturCode { get; set; }
        public DateTime FakturDate { get; set; }
        public DateTime JatuhTempo { get; set; }
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Alamat { get; set; }
        public decimal NilaiTotal { get; set; }
        public decimal NilaiTerbayar { get; set; }
        public decimal NilaiTagih { get; set; }
        public string FakturId { get; set; }

        public bool IsTandaTerima { get; set; }
        public string Keterangan { get; set; }
        public DateTime TandaTerimaDate { get; set; }
    }
}
