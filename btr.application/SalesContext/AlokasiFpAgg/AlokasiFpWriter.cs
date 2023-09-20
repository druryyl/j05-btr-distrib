using btr.domain.SalesContext.FakturPajak;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.NomorFpAgg
{
    public interface IAlokasiFpWriter : INunaWriter2<AlokasiFpModel>
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
    }
}
