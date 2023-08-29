using btr.domain.SupportContext.DocAgg;
using btr.domain.SupportContext.PrintManagerAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SupportContext.DocAgg
{
    public interface IDocActionDal :
        IInsertBulk<DocActionModel>,
        IDelete<IDocKey>,
        IListData<DocActionModel, IDocKey>
    {
    }
}
