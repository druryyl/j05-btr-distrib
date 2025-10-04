using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.FinanceContext.TagihanAgg
{
    public class TagihanModel : ITagihanKey
    {
        public TagihanModel()
        {
        }
        public TagihanModel(string id) => TagihanId = id;

        public string TagihanId { get; set; }
        public DateTime TagihanDate { get; set; }
        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
        public decimal TotalTagihan { get; set; }
        public List<TagihanFakturModel> ListFaktur { get; set; }
        public void TandaTerima(string fakturId)
        {
            var faktur = ListFaktur.FirstOrDefault(x => x.FakturId == fakturId);
            if (faktur != null)
            {
                faktur.IsTandaTerima = true;
                faktur.TandaTerimaDate = DateTime.Now;
            }
        }
        public void BatalTandaTerima(string fakturId)
        {
            var faktur = ListFaktur.FirstOrDefault(x => x.FakturId == fakturId);
            if (faktur != null)
            {
                faktur.IsTandaTerima = false;
                faktur.TandaTerimaDate = new DateTime(3000, 1, 1);
            }
        }

        public void TagihUlang(string fakturId)
        {
            var faktur = ListFaktur.FirstOrDefault(x => x.FakturId == fakturId);
            if (faktur != null)
                faktur.IsTagihUlang = true;
        }
        public void BatalTagihUlang(string fakturId)
        {
            var faktur = ListFaktur.FirstOrDefault(x => x.FakturId == fakturId);
            if (faktur != null)
                faktur.IsTagihUlang = false;
        }
    }

    public interface ITagihanKey
    {
        string TagihanId { get;}
    }

    public class TagihanFakturModel : ITagihanKey
    {
        public string TagihanId { get; set; }
        public int NoUrut { get; set; }
        public string FakturId { get; set; }
        public string FakturCode { get; set; }
        public DateTime FakturDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Alamat { get; set; }
        public decimal NilaiTotal { get; set; }
        public decimal NilaiTerbayar { get; set; }
        public decimal NilaiTagih { get; set; }
        public bool IsTandaTerima { get; set; }
        public string Keterangan { get; set; }
        public DateTime TandaTerimaDate { get; set; }
        public bool IsTagihUlang { get; set; }
    }
}
