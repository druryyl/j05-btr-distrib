using System.Collections.Generic;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.application.SalesContext.WilayahAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.SalesContext.SalesPersonAgg.Workers
{
    public interface ISalesPersonBuilder : INunaBuilder<SalesPersonModel>
    {
        ISalesPersonBuilder CreateNew();
        ISalesPersonBuilder Load(ISalesPersonKey salesPersonKey);
        ISalesPersonBuilder Attach(SalesPersonModel model);
        ISalesPersonBuilder Wilayah(IWilayahKey wilayahKey);
        ISalesPersonBuilder Name(string name);
    }

    public class SalesPersonBuilder : ISalesPersonBuilder
    {
        private SalesPersonModel _aggRoot = new SalesPersonModel();
        private readonly ISalesPersonDal _salesPersonDal;
        private readonly IWilayahDal _wilayahDal;

        public SalesPersonBuilder(ISalesPersonDal salesPersonDal, 
            IWilayahDal wilayahDal)
        {
            _salesPersonDal = salesPersonDal;
            _wilayahDal = wilayahDal;
        }

        public SalesPersonModel Build()
        {
            _aggRoot.RemoveNull();
            return _aggRoot;
        }

        public ISalesPersonBuilder CreateNew()
        {
            _aggRoot = new SalesPersonModel
            {
                SalesPersonName = string.Empty,
                SalesPersonId = string.Empty
            };
            return this;
        }


        public ISalesPersonBuilder Load(ISalesPersonKey salesPersonKey)
        {
            _aggRoot = _salesPersonDal.GetData(salesPersonKey)
                       ?? throw new KeyNotFoundException($"SalesPersonId not found ({salesPersonKey.SalesPersonId})");
            return this;
        }

        public ISalesPersonBuilder Name(string name)
        {
            _aggRoot.SalesPersonName = name;
            return this;
        }

        public ISalesPersonBuilder Attach(SalesPersonModel model)
        {
            _aggRoot = model;
            return this;
        }

        public ISalesPersonBuilder Wilayah(IWilayahKey wilayahKey)
        {
            if (wilayahKey.WilayahId == string.Empty)
            {
                _aggRoot.WilayahId = string.Empty;
                _aggRoot.WilayahName = string.Empty;
                return this;
            }

            var wilayah = _wilayahDal.GetData(wilayahKey)
                ?? throw new KeyNotFoundException("WilayahID invalid");
            _aggRoot.WilayahId = wilayah.WilayahId;
            _aggRoot.WilayahName = wilayah.WilayahName;
            return this;
        }
    }
}