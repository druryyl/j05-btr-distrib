using btr.application.BrgContext.BrgAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokBalanceAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;

namespace btr.application.InventoryContext.StokBalanceAgg
{
    public interface IStokBalanceBuilder : INunaBuilder<StokBalanceModel>
    {
        IStokBalanceBuilder Load(IBrgKey brgKey);
        IStokBalanceBuilder Qty(IWarehouseKey warehouseKey, int qty);
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

        public IStokBalanceBuilder Qty(IWarehouseKey warehouseKey, int qty)
        {
            var wh = _agg.ListWarehouse
                .FirstOrDefault(x => x.WarehouseId == warehouseKey.WarehouseId);
            if (wh is null)
            {
                wh = new StokBalanceWarehouseModel 
                { 
                    WarehouseId = warehouseKey.WarehouseId,
                    Qty = qty,
                };
                _agg.ListWarehouse.Add(wh);
            }
            else
                wh.Qty = qty;

            return this;

        }
    }
}
