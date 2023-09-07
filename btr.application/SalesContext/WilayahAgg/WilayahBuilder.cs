using btr.nuna.Application;
using btr.nuna.Domain;
using System.Collections.Generic;
using btr.domain.SalesContext.WilayahAgg;

namespace btr.application.SalesContext.WilayahAgg
{
    public interface IWilayahBuilder : INunaBuilder<WilayahModel>
    {
        IWilayahBuilder Create();
        IWilayahBuilder Load(IWilayahKey wilayahKey);
        IWilayahBuilder Attach(WilayahModel wilayah);
        IWilayahBuilder Name(string name);
    }

    public class WilayahBuilder : IWilayahBuilder
    {
        private WilayahModel _agg = new WilayahModel();
        private readonly IWilayahDal _wilayahDal;

        public WilayahBuilder(IWilayahDal wilayahDal)
        {
            _wilayahDal = wilayahDal;
        }

        public WilayahModel Build()
        {
            _agg.RemoveNull();
            return _agg;
        }

        public IWilayahBuilder Create()
        {
            _agg = new WilayahModel();
            return this;
        }

        public IWilayahBuilder Load(IWilayahKey wilayahKey)
        {
            _agg = _wilayahDal.GetData(wilayahKey)
                ?? throw new KeyNotFoundException("WilayahId invalid");
            return this;
        }

        public IWilayahBuilder Attach(WilayahModel wilayah)
        {
            _agg = wilayah;
            return this;
        }

        public IWilayahBuilder Name(string name)
        {
            _agg.WilayahName = name;
            return this;
        }
    }
}
