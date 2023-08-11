using btr.domain.BrgContext.BrgAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.BrgContext.BrgAgg.Contracts
{
    public interface IBrgHarga :
        IInsertBulk<BrgHargaModel>,
        IDelete<IBrgKey>,
        IListData<BrgHargaModel, IBrgKey>
    {
    }
}
