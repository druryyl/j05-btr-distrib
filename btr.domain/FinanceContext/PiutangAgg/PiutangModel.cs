using System;
using System.Collections.Generic;

namespace btr.domain.FinanceContext.PiutangAgg
{
    public class PiutangModel : IPiutangKey
    {
        public PiutangModel()
        {
        }
        public PiutangModel(string id)
        {
            PiutangId = id;
        }
        public string PiutangId { get; set; }
        public DateTime PiutangDate { get; set; }
        public DateTime DueDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal Total { get; set; }
        public decimal Terbayar { get; set; }
        public decimal Sisa { get; set; }
        public List<PiutangElementModel> ListElement { get; set; }
        public List<PiutangLunasModel> ListLunas { get; set; }
    }

    public class PiutangElementModel
    {
        public string PiutangId { get; set; }
        public int NoUrut { get; set; }
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
        public int NoUrut { get; set; }
        public DateTime LunasDate { get; set; }
        public decimal Nilai { get;set; }
        public JenisLunasEnum JenisLunas { get; set; }
    }
}