using System;
using System.ComponentModel;
using System.Linq;

namespace btr.distrib.FinanceContext.ReturBalanceAgg
{
    public class FakturPotHeaderViewDto : IReCalc
    {
        public FakturPotHeaderViewDto()
        {
            NilaiRetur = 0;
            ListDetil = new BindingList<FakturPotDetilViewDto>();
        }
        public decimal NilaiRetur { get; private set; }
        public decimal NilaiSumPost => ListDetil.Sum(x => x.NilaiPosting);
        public decimal NilaiBalance => NilaiRetur - NilaiSumPost;

        public void SetNilaiRetur(decimal nilaiRetur) => NilaiRetur = nilaiRetur;
        public void ReCalc()
        {
            foreach (var item in ListDetil)
                item.SetNilaiBalance(NilaiBalance);
        }
        public BindingList<FakturPotDetilViewDto> ListDetil { get; set; }
        public void AddItem(FakturPotDetilViewDto item)
        {
            ListDetil.Add(item);
            item.SetReCalc(this);
            ReCalc();
        }
    }

    public interface IReCalc
    {
        void ReCalc();
    }

    public class FakturPotDetilViewDto
    {
        private bool _isPost;
        private IReCalc _reCalc;
        private decimal _nilaiPosting;

        public FakturPotDetilViewDto(int no, string fakturId, string fakturCode,
            string fakturDate, decimal nilaiFaktur, decimal nilaiPotongan, 
            decimal nilaiPosting, bool isHeapFaktur)
        {
            No = no;
            FakturId = fakturId;
            FakturCode = fakturCode;
            FakturDate = fakturDate;
            NilaiFaktur = nilaiFaktur;
            NilaiPotongan = nilaiPotongan;
            NilaiSisaPotong = 0;

            _nilaiPosting = nilaiPosting;
            _isPost = nilaiPosting == 0 ? false : true;
            IsHeapFaktur = isHeapFaktur;
        }

        public int No { get; private set; }
        public string FakturId { get; private set; }
        public string FakturCode { get; private set; }
        public string FakturDate { get; private set; }
        public bool IsHeapFaktur { get; private set; }

        public decimal NilaiFaktur { get; private set; }
        public decimal NilaiPotongan { get; private set; }
        public decimal NilaiSisaPotong { get; private set; }
        public bool IsPost 
        {
            get => _isPost;
            set
            {
                _isPost = value;
                if (!_isPost)
                {
                    _nilaiPosting = 0;
                    return;
                }
                _nilaiPosting = Math.Min(NilaiSisaPotong, NilaiBalance);
                AskHeaderToReCalc();
            }
        }

        public decimal NilaiPosting 
        { 
            get => _nilaiPosting;
        }
        public decimal NilaiBalance { get; private set; }
        private void AskHeaderToReCalc()
        {
            _reCalc.ReCalc();
        }
        public void SetNilaiBalance(decimal nilai)
        {
            NilaiBalance = nilai;
        }
        public void SetReCalc(IReCalc reCalc)
            => _reCalc = reCalc;
        private void SetNilaiSisaPotong(decimal nilaiSisaPotong)
            => NilaiSisaPotong = nilaiSisaPotong;
    }
}
