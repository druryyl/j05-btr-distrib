using btr.domain.SupportContext.PrintManagerAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SupportContext.PrintManagerAgg
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
