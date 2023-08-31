using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using btr.domain.SalesContext.WilayahAgg;

namespace btr.application.SalesContext.WilayahAgg
{
    public interface IWilayahWriter : INunaWriter<WilayahModel>
    { 
    }

    public class WilayahWriter : IWilayahWriter
    {
        private readonly IWilayahDal _wilayahDal;
        private readonly INunaCounterBL _counter;

        public WilayahWriter(IWilayahDal wilayahDal, 
            INunaCounterBL counter)
        {
            _wilayahDal = wilayahDal;
            _counter = counter;
        }

        public void Save(ref WilayahModel model)
        {
            if (model.WilayahId.IsNullOrEmpty())
                model.WilayahId = _counter.Generate("W", IDFormatEnum.Pnn);

            var wilayahDb = _wilayahDal.GetData(model);
            if (wilayahDb is null)
                _wilayahDal.Insert(model);
            else
                _wilayahDal.Update(model);
        }
    }
}
