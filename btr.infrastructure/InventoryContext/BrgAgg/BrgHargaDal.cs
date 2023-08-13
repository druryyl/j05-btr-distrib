using btr.application.BrgContext.BrgAgg.Contracts;
using btr.domain.BrgContext.BrgAgg;
using System;
using System.Collections.Generic;

namespace btr.infrastructure.InventoryContext.BrgAgg
{
    public class BrgHargaDal : IBrgHargaDal
    {
        public void Insert(IEnumerable<BrgHargaModel> listModel)
        {
            throw new NotImplementedException();
        }

        public void Delete(IBrgKey key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BrgHargaModel> ListData(IBrgKey filter)
        {
            throw new NotImplementedException();
        }
    }
}
