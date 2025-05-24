using btr.domain.BrgContext.KategoriAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System.Collections.Generic;

namespace btr.application.BrgContext.KategoriAgg
{
    public interface IKategoriBuilder : INunaBuilder<KategoriModel>
    {
        IKategoriBuilder Create();
        IKategoriBuilder Load(IKategoriKey kategoriKey);
        IKategoriBuilder Attach(KategoriModel model);
        IKategoriBuilder Name(string name);
        IKategoriBuilder Code(string code);
        IKategoriBuilder Supplier(string supplierId, string supplierName);
    }

    public class KategoriBuilder : IKategoriBuilder
    {
        private KategoriModel _agg;
        private readonly IKategoriDal _kategoriDal;

        public KategoriBuilder(IKategoriDal kategoriDal)
        {
            _kategoriDal = kategoriDal;
        }

        public KategoriModel Build()
        {
            _agg.RemoveNull();
            return _agg;
        }

        public IKategoriBuilder Create()
        {
            _agg = new KategoriModel();
            return this;
        }

        public IKategoriBuilder Load(IKategoriKey kategoriKey)
        {
            _agg = _kategoriDal.GetData(kategoriKey)
                ?? throw new KeyNotFoundException("KategoriId invalid");
            return this;
        }

        public IKategoriBuilder Attach(KategoriModel model)
        {
            _agg = model;
            return this;
        }

        public IKategoriBuilder Name(string name)
        {
            _agg.KategoriName = name;
            return this;
        }

        public IKategoriBuilder Code(string code)
        {
            _agg.Code = code;
            return this;
        }

        public IKategoriBuilder Supplier(string supplierId, string supplierName)
        {
            _agg.SupplierId = supplierId;
            _agg.SupplierName = supplierName;
            return this;
        }
    }
}
