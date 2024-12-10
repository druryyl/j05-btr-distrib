using DocumentFormat.OpenXml.Drawing;
using System;
using System.ComponentModel;

namespace btr.distrib.FinanceContext.ReturBalanceAgg
{
    public class FakturPotHeaderViewDto
    {
        private decimal _nilaiReturBalance;
        public decimal NilaiReturBalance 
        {
            get => _nilaiReturBalance;
            set
            {
                _nilaiReturBalance = value;
                if (ListDetil is null)
                    return;
                foreach(var item in ListDetil)
                    item.SetNilaiReturBalance(value);
            }
        }
        public BindingList<FakturPotDetilViewDto> ListDetil { get; set; }
    }

    public class FakturPotDetilViewDto
    {
        private bool _post;
        public FakturPotDetilViewDto(int no, string fakturId, string fakturCode,
            string fakturDate, decimal nilaiFaktur, decimal nilaiPotongan, 
            decimal nilaiPosting, bool post)
        {
            No = no;
            FakturId = fakturId;
            FakturCode = fakturCode;
            FakturDate = fakturDate;
            NilaiFaktur = nilaiFaktur;
            NilaiPotongan = nilaiPotongan;
            NilaiPosting = nilaiPosting;
            _post = post;
        }

        public int No { get; private set; }
        public string FakturId { get; private set; }
        public string FakturCode { get; private set; }
        public string FakturDate { get; private set; }

        public decimal NilaiFaktur { get; private set; }
        public decimal NilaiPotongan { get; private set; }
        public bool Post 
        {
            get => _post;
            set
            {
                _post = value;
                if (!_post)
                {
                    NilaiPosting = 0;
                    return;
                }
                NilaiPosting = Math.Min(NilaiPotongan, NilaiReturBalance);
                NilaiReturBalance -= NilaiPosting;
            }
        }
        public decimal NilaiPosting { get; set; }
        public decimal NilaiReturBalance { get; private set; }
        public void SetNilaiReturBalance(decimal nilai)
        {
            NilaiReturBalance = nilai;
        }
    }
}
