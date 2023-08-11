using System.Collections.Generic;
using System.Linq;
using btr.application.InventoryContext.BrgAgg.Contracts;
using btr.domain.BrgContext.BrgAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.InventoryContext.BrgAgg.Workers
{
    public interface IBrgBuilder : INunaBuilder<BrgModel>
    {
        IBrgBuilder Load(IBrgKey brgKey);
    }

    public class BrgBuilder : IBrgBuilder
    {
        private BrgModel _aggRoot = new BrgModel();
        private readonly IBrgDal _brgDal;
        private readonly IBrgSatuanDal _brgSatuanHargaDal;

        public BrgBuilder(IBrgDal brgDal,
            IBrgSatuanDal brgSatuanHargaDal)
        {
            _brgDal = brgDal;
            _brgSatuanHargaDal = brgSatuanHargaDal;
        }

        public BrgModel Build()
        {
            _aggRoot.RemoveNull();
            return _aggRoot;
        }

        public IBrgBuilder Load(IBrgKey brgKey)
        {
            _aggRoot = _brgDal.GetData(brgKey)
                       ?? throw new KeyNotFoundException($"BrgId not found ({brgKey.BrgId})");
            _aggRoot.ListSatuanHarga = _brgSatuanHargaDal.ListData(brgKey)?.ToList()
                                       ?? new List<BrgSatuanModel>();
            return this;
        }
    }
}