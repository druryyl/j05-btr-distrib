using btr.domain.FinanceContext.TagihanAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;

namespace btr.application.FinanceContext.TagihanAgg
{
    public interface  ITagihanDal :
        IInsert<TagihanModel>,
        IUpdate<TagihanModel>,
        IDelete<TagihanModel>,
        IGetData<TagihanModel, ITagihanKey>,
        IListData<TagihanModel, Periode>
    {
    }

    public interface ITagihanFakturDal :
        IInsertBulk<TagihanFakturModel>,
        IDelete<ITagihanKey>,
        IListData<TagihanFakturModel, ITagihanKey>,
        IListData<TagihanFakturViewDto, IFakturKey>,
        IListData<TandaTerimaTagihanViewDto, Periode>
    {
        IEnumerable<TagihanFakturModel> ListDataByFaktur(IFakturKey fakturKey);
        void Update(TagihanFakturModel model, ITagihanKey tagihan, IFakturKey faktur);
    }

    public class TagihanFakturViewDto
    {
        public string TagihanId { get; set; }
        public string FakturId { get; set; }
        public DateTime TagihanDate { get; set; }
        public string SalesPersonName { get; set; }
    }

    public class TandaTerimaTagihanViewDto
    {
        public string TagihanId { get; private set; }
        public string SalesPersonId { get; private set; }
        public string SalesPersonName { get; private set; }
        public string FakturId { get; private set; }
        public string FakturCode { get; private set; }
        public DateTime FakturDate { get; private set; }
        public string CustomerId { get; private set; }
        public string CustomerName { get; private set; }
        public string Alamat { get; private set; }
        public decimal NilaiTotal { get; private set; }
        public decimal NilaiTerbayar { get; private set; }
        public decimal NilaiTagih { get; private set; }
        public decimal NilaiPelunasan { get; private set; }

        public bool IsTandaTerima { get; set; }
        public bool IsTagihUlang { get; set; }
        
        public string Keterangan { get; set; }
        public DateTime TandaTerimaDate { get; set; }
    }
}