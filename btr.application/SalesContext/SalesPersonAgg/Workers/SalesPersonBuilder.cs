using System.Collections.Generic;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.SalesContext.SalesPersonAgg.Workers
{
    public interface ISalesPersonBuilder : INunaBuilder<SalesPersonModel>
    {
        ISalesPersonBuilder CreateNew();
        ISalesPersonBuilder Load(ISalesPersonKey salesPersonKey);
        ISalesPersonBuilder Name(string name);
    }

    public class SalesPersonBuilder : ISalesPersonBuilder
    {
        private SalesPersonModel _aggRoot = new SalesPersonModel();
        private readonly ISalesPersonDal _salesPersonDal;

        public SalesPersonBuilder(ISalesPersonDal salesPersonDal)
        {
            _salesPersonDal = salesPersonDal;
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
    }
}