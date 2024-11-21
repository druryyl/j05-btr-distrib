using btr.domain.FinanceContext.TagihanAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;

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
        IListData<TagihanFakturViewDto, IFakturKey>
    {
    }

    public class TagihanFakturViewDto
    {
        public string TagihanId { get; set; }
        public string FakturId { get; set; }
        public DateTime TagihanDate { get; set; }
        public string SalesPersonName { get; set; }
    }
}