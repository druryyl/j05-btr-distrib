﻿using btr.domain.SalesContext.AlokasiFpAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.SalesContext.AlokasiFpAgg
{
    public interface IAlokasiFpWriter : 
        INunaWriter2<AlokasiFpModel>,
        INunaDelete<AlokasiFpModel>
    {
    }

    public class AlokasiFpWriter : IAlokasiFpWriter
    {
        private readonly INunaCounterBL _counter;
        private readonly IAlokasiFpDal _alokasiFpDal;
        private readonly IAlokasiFpItemDal _alokasiFpItemDal;

        public AlokasiFpWriter(INunaCounterBL counter,
            IAlokasiFpDal alokasiFpDal,
            IAlokasiFpItemDal alokasiFpItemDal)
        {
            _counter = counter;
            _alokasiFpDal = alokasiFpDal;
            _alokasiFpItemDal = alokasiFpItemDal;
        }

        public AlokasiFpModel Save(AlokasiFpModel model)
        {
            if (model.AlokasiFpId.IsNullOrEmpty())
                model.AlokasiFpId = _counter.Generate("ALFP", IDFormatEnum.PREFYYMnnnnnC);
            model.ListItem.ForEach(x => x.AlokasiFpId = model.AlokasiFpId);

            var db = _alokasiFpDal.GetData(model);
            
            using (var trans = TransHelper.NewScope())
            { 
                if (db is null)
                    _alokasiFpDal.Insert(model);
                else
                    _alokasiFpDal.Update(model);

                _alokasiFpItemDal.Delete(model);
                _alokasiFpItemDal.Insert(model.ListItem);
                trans.Complete();
            }
            return model;
        }

        public void Delete(AlokasiFpModel model)
        {
            using(var trans = TransHelper.NewScope())
            {
                _alokasiFpItemDal.Delete(model);
                _alokasiFpDal.Delete(model);
                trans.Complete();
            }
        }
    }
}
