using btr.application.InventoryContext.ReturJualAgg.Contracts;
using btr.domain.InventoryContext.ReturJualAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.ReturJualAgg.Workers
{
    public interface IReturJualBuilder : INunaBuilder<ReturJualModel>
    {
        IReturJualBuilder Load(IReturJualKey returJualKey);
    }

    public class ReturJualBuilder : IReturJualBuilder
    {
        private ReturJualModel _aggregate;
        private readonly IReturJualDal _returJualDal;
        private readonly IReturJualItemDal _returJualItemDal;
        private readonly IReturJualItemQtyHrgDal _returJualItemQtyHrgDal;
        private readonly IReturJualItemDiscDal _returJualItemDiscDal;

        public ReturJualBuilder(IReturJualDal returJualDal, 
            IReturJualItemDal returJualItemDal, 
            IReturJualItemQtyHrgDal returJualItemQtyHrgDal, 
            IReturJualItemDiscDal returJualItemDiscDal)
        {
            _returJualDal = returJualDal;
            _returJualItemDal = returJualItemDal;
            _returJualItemQtyHrgDal = returJualItemQtyHrgDal;
            _returJualItemDiscDal = returJualItemDiscDal;
        }


        public IReturJualBuilder Load(IReturJualKey returJualKey)
        {
            _aggregate = _returJualDal.GetData(returJualKey)
                ?? throw new Exception("Retur Jual tidak ditemukan");
            _aggregate.ListItem = _returJualItemDal.ListData(returJualKey)?.ToList()
                ?? new List<ReturJualItemModel>();
            var listQtyHrg = _returJualItemQtyHrgDal.ListData(returJualKey)?.ToList()
                ?? new List<ReturJualItemQtyHrgModel>();
            var listDisc = _returJualItemDiscDal.ListData(returJualKey)?.ToList()
                ?? new List<ReturJualItemDiscModel>();

            _aggregate.ListItem.ForEach(item =>
            {
                item.ListQtyHrg = listQtyHrg.Where(x => x.ReturJualItemId == item.ReturJualItemId).ToList();
                item.ListDisc = listDisc.Where(x => x.ReturJualItemId == item.ReturJualItemId).ToList();
            });
            return this;
        }

        public ReturJualModel Build()
        {
            _aggregate.RemoveNull();
            return _aggregate;
        }
    }
}
