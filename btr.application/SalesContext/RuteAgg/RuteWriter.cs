using btr.domain.SalesContext.RuteAgg;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.RuteAgg
{
    public interface IRuteWriter : INunaWriter2<RuteModel>
    {
    }
    public class RuteWriter : IRuteWriter
    {
        private readonly IRuteDal _ruteDal;
        private readonly IRuteItemDal _ruteCustomerDal;
        private readonly INunaCounterBL _counter;

        public RuteWriter(IRuteDal ruteDal, 
            IRuteItemDal ruteCustomerDal, 
            INunaCounterBL counter)
        {
            _ruteDal = ruteDal;
            _ruteCustomerDal = ruteCustomerDal;
            _counter = counter;
        }


        public RuteModel Save(RuteModel model)
        {
            if (model.RuteId == string.Empty)
                model.RuteId = _counter.Generate("RT", IDFormatEnum.PFnnn);
            var noUrut = 0;
            foreach(var item in model.ListCustomer)
            {
                noUrut++;
                item.RuteId = model.RuteId;
                item.NoUrut = noUrut;
            }

            var db = _ruteDal.GetData(model);
            using (var trans = TransHelper.NewScope())
            {
                if (db is null)
                    _ruteDal.Insert(model);
                else
                    _ruteDal.Update(model);

                _ruteCustomerDal.Delete(model);
                _ruteCustomerDal.Insert(model.ListCustomer);
                trans.Complete();
            }
            return model;
        }
    }
}
