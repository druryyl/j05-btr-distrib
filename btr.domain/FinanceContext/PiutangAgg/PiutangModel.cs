using System;

namespace btr.domain.FinanceContext.PiutangAgg
{
    public class PiutangModel : IPiutangKey
    {
        public string PiutangId { get; set; }
        public DateTime PiutangDate { get; set; }
        public DateTime JatuhTempo { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal Total { get; set; }
        public decimal Terbayar { get; set; }
        public decimal Sisa { get; set; }

        public string UserId { get; set; }
    }

    public class PiutangElementModel
    {
        public string PiutangId { get; set; }
        public string ElementName { get; set; }
        public decimal NilaiPlus { get; set; }
        public decimal NilaiMinus { get; set; }
    }

    public enum JenisLunasEnum
    {
        Cash, CekBg
    }

    public class PiutangLunasModel
    {
        public string PiutangId { get; set; }
        public DateTime LunasDate { get; set; }
        public decimal Nilai { get;set; }
        public JenisLunasEnum JenisLunas { get; set; }
    }
}