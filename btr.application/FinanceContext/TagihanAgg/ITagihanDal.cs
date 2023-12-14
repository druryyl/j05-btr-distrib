using btr.domain.FinanceContext.TagihanAgg;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.FinanceContext.TagihanAgg
{
    public interface ITagihanDal : 
        IInsert<TagihanModel>,
        IUpdate<TagihanModel>,
        IDelete<TagihanModel>,
        IGetData<TagihanModel, ITagihanKey>,
        IListData<TagihanModel, Periode>
    {
    }
}
