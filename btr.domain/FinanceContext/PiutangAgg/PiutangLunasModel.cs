using btr.domain.FinanceContext.TagihanAgg;
using System;

namespace btr.domain.FinanceContext.PiutangAgg
{
    public class PiutangLunasModel : IPiutangKey, ITagihanKey
    {
        public string PiutangId { get; set; }
        public int NoUrut { get; set; }
        public string PelunasanId { get; set; }
        public DateTime LunasDate { get; set; }
        public string TagihanId { get; set; }
        public decimal Nilai { get;set; }
        public JenisLunasEnum JenisLunas { get; set; }
        public DateTime JatuhTempoBg { get; set; }
        public string NoRekBg { get; set; }
        public string NamaBank { get; set; }
        public string AtasNamaBank { get; set; }

    }
}