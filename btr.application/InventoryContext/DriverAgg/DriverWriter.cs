using btr.application.InventoryContext.MutasiAgg;
using btr.domain.InventoryContext.DriverAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.DriverAgg
{
    public interface IDriverWriter : INunaWriter2<DriverModel>
    {
    }

    public class DriverWriter : IDriverWriter
    {
        private readonly IDriverDal _driverDal;
        private readonly INunaCounterBL _counter;


        public DriverWriter(IDriverDal driverDal, 
            INunaCounterBL counter)
        {
            _driverDal = driverDal;
            _counter = counter;
        }

        public DriverModel Save(DriverModel model)
        {
            if (model is null)
                throw new ArgumentNullException("Driver data is null. Save failed");

            //  GENERATE-ID
            if (model.DriverId.IsNullOrEmpty())
                model.DriverId = _counter.Generate("DV", IDFormatEnum.PFnnn);

            //  WRITE
            using (var trans = TransHelper.NewScope())
            {
                var db = _driverDal.GetData(model);
                if (db is null)
                    _driverDal.Insert(model);
                else
                    _driverDal.Update(model);

                trans.Complete();
            }
            return model;


        }
    }
}
