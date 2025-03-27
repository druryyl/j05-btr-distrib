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

        public string FakturCode { get; set; }
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public decimal Total { get; set; }
        public decimal Potongan { get; set; }
        public decimal Terbayar { get; set; }
        public decimal Sisa { get; set; }
        public List<PiutangElementModel> ListElement { get; set; }
        public List<PiutangLunasModel> ListLunas { get; set; }
    }
    
    public enum PiutangElementEnum
    {
        NilaiAwalPiutang,
        
        Retur,
        Potongan,
        Materai,
        Admin,
    }
}