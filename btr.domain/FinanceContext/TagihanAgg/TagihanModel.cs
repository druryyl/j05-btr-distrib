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
    }
}
