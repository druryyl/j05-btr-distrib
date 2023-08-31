using btr.domain.SupportContext.DocAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;

namespace btr.application.SupportContext.DocAgg
{
    public interface IDocDal :
        IInsert<DocModel>,
        IUpdate<DocModel>,
        IDelete<IDocKey>,
        IGetData<DocModel, IDocKey>,
        IListData<DocModel, Periode>
    {
    }
}
