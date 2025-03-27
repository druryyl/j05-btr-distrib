using btr.application.SalesContext.SalesPersonAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.SalesRuteAgg
{
    public interface ISalesRuteWriter : INunaWriter2<SalesRuteModel>
    {
    }
    public class SalesRuteWriter : ISalesRuteWriter
    {
        private readonly ISalesRuteDal _salesRuteDal;
        private readonly ISalesRuteItemDal _salesRuteItemDal;
        private readonly INunaCounterBL _counter;

        public SalesRuteWriter(ISalesRuteDal salesRuteDal, 
            ISalesRuteItemDal salesRuteItemDal, 
            INunaCounterBL counter)
        {
            _salesRuteDal = salesRuteDal;
            _salesRuteItemDal = salesRuteItemDal;
            _counter = counter;
        }

        public SalesRuteModel Save(SalesRuteModel model)
        {
            if (model.SalesRuteId == string.Empty)
                model.SalesRuteId = _counter.Generate("RT", IDFormatEnum.PFnnn);
            model.ListCustomer.ForEach(x => x.SalesRuteId = model.SalesRuteId);

            using (var trans = TransHelper.NewScope())
            {
                var db = _salesRuteDal.GetData(model);
                if (db is null)
                    _salesRuteDal.Insert(model);
                else
                    _salesRuteDal.Update(model);

                _salesRuteItemDal.Delete(model);
                _salesRuteItemDal.Insert(model.ListCustomer);
                trans.Complete();
            }

            return model;
        }
    }
}
