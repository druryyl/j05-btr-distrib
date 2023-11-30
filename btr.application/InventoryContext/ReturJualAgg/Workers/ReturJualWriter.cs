using btr.application.InventoryContext.ReturJualAgg.Contracts;
using btr.domain.InventoryContext.ReturJualAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.InventoryContext.ReturJualAgg.Workers
{
    public interface IReturJualWriter : INunaWriter2<ReturJualModel>
    {
    }

    public class ReturJualWriter : IReturJualWriter
    {
        private readonly IReturJualDal _returJualDal;
        private readonly IReturJualItemDal _returJualItemDal;
        private readonly IReturJualItemQtyHrgDal _returJualItemQtyHrgDal;
        private readonly IReturJualItemDiscDal _returJualItemDiscDal;
        private readonly INunaCounterBL _counter;

        public ReturJualWriter(IReturJualDal returJualDal,
            IReturJualItemDal returJualItemDal,
            IReturJualItemQtyHrgDal returJualItemQtyHrgDal,
            IReturJualItemDiscDal returJualItemDiscDal,
            INunaCounterBL counter)
        {
            _returJualDal = returJualDal;
            _returJualItemDal = returJualItemDal;
            _returJualItemQtyHrgDal = returJualItemQtyHrgDal;
            _returJualItemDiscDal = returJualItemDiscDal;
            _counter = counter;
        }


        public ReturJualModel Save(ReturJualModel model)
        {
            if (model.ReturJualId.IsNullOrEmpty())
                model.ReturJualId = _counter.Generate("RETJ", IDFormatEnum.PREFYYMnnnnnC);

            var returJualId = model.ReturJualId;
            model.ListItem.ForEach(x =>
            {
                x.ReturJualId = returJualId;
                x.ReturJualItemId = $"{returJualId}-{x.NoUrut:D2}";
                // x.ListQtyHrg.ForEach(y =>
                // {
                //     y.ReturJualId = returJualId;
                //     y.ReturJualItemId = x.ReturJualItemId;
                //     y.ReturJualItemQtyHrgId = $"{returJualId}-{x.NoUrut:D2}-{y.NoUrut:D2}";
                // });
                // x.ListDisc.ForEach(y =>
                // {
                //     y.ReturJualId = returJualId;
                //     y.ReturJualItemId = x.ReturJualItemId;
                //     y.ReturJualItemDiscId = $"{returJualId}-{x.NoUrut:D2}-{y.NoUrut:D2}";
                // });
            });

            var db = _returJualDal.GetData(model);
            
            using(var trans = TransHelper.NewScope())
            {
                if (db is null)
                    _returJualDal.Insert(model);
                else
                    _returJualDal.Update(model);

                _returJualItemDal.Delete(model);
                _returJualItemDal.Insert(model.ListItem);

                // _returJualItemQtyHrgDal.Delete(model);
                // _returJualItemQtyHrgDal.Insert(model.ListItem.SelectMany(x => x.ListQtyHrg));
                //
                // _returJualItemDiscDal.Delete(model);
                // _returJualItemDiscDal.Insert(model.ListItem.SelectMany(x => x.ListDisc));
                trans.Complete();
            }
            return model;
        }
    }
}
