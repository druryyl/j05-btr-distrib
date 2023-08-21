using btr.application.InventoryContext.BrgAgg.Contracts;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokBalanceAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;

namespace btr.application.InventoryContext.StokBalanceAgg
{
    public interface IStokBalanceBuilder : INunaBuilder<StokBalanceModel>
    {
        IStokBalanceBuilder Load(IBrgKey brgKey);
    }
    public class StokBalanceBuilder : IStokBalanceBuilder
    {
        private StokBalanceModel _agg;
        private readonly IStokBalanceWarehouseDal _stokBalanceWarehouseDal;
        private readonly IBrgDal _brgDal;

        public StokBalanceBuilder(IStokBalanceWarehouseDal stokBalanceWarehouseDal, 
            IBrgDal brgDal)
        {
            _stokBalanceWarehouseDal = stokBalanceWarehouseDal;
            _brgDal = brgDal;
        }

        public IStokBalanceBuilder Load(IBrgKey brgKey)
        {
            var brg = _brgDal.GetData(brgKey)
                ?? throw new KeyNotFoundException("BrgId invalid");
            _agg = new StokBalanceModel
            {
                BrgId = brg.BrgId,
                BrgName = brg.BrgName,
                ListWarehouse = _stokBalanceWarehouseDal.ListData(brgKey)?.ToList()
                ?? new List<StokBalanceWarehouseModel>()
            };
            return this;
        }

        public StokBalanceModel Build()
        {
            _agg.RemoveNull();
            return _agg;
        }
    }
}
