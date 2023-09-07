using btr.domain.SupportContext.DocAgg;
using btr.nuna.Infrastructure;

namespace btr.application.SupportContext.DocAgg
{
    public interface IDocActionDal :
        IInsertBulk<DocActionModel>,
        IDelete<IDocKey>,
        IListData<DocActionModel, IDocKey>
    {
    }
}
