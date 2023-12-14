using btr.domain.FinanceContext.TagihanAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;

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
        IListData<TagihanFakturModel, ITagihanKey>
    {
    }
}