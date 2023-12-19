using System;
using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.AdjustmentAgg;
using btr.domain.InventoryContext.StokBalanceAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.InventoryContext.AdjustmentAgg
{
    public interface IAdjustmentBuilder : INunaBuilder<AdjustmentModel>
    {
        IAdjustmentBuilder Create();
        IAdjustmentBuilder Load(IAdjustmentKey adjustmentKey);
        IAdjustmentBuilder Brg(IBrgKey brgKey);
        IAdjustmentBuilder Warehouse(IWarehouseKey warehouseKey);
        IAdjustmentBuilder Alasan(string alasan);
        IAdjustmentBuilder AdjustmentDate(DateTime adjustmentDate);
        IAdjustmentBuilder QtyAdjustInPcs(int qtyAdjustInPcs);
    }

    public class AdjustmnetBuilder : IAdjustmentBuilder
    {
        private AdjustmentModel _aggregate = new AdjustmentModel();
        private readonly IAdjustmentDal _adjustmentDal;
        private readonly IBrgDal _brgDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IStokBalanceWarehouseDal _stokBalanceWarehouseDal;
        private readonly IBrgBuilder _brgBuilder;

        public AdjustmnetBuilder(IAdjustmentDal adjustmentDal, 
            IBrgDal brgDal, 
            IWarehouseDal warehouseDal, 
            IStokBalanceWarehouseDal stokBalanceWarehouseDal, 
            IBrgBuilder brgBuilder)
        {
            _adjustmentDal = adjustmentDal;
            _brgDal = brgDal;
            _warehouseDal = warehouseDal;
            _stokBalanceWarehouseDal = stokBalanceWarehouseDal;
            _brgBuilder = brgBuilder;
        }

        public AdjustmentModel Build()
        {
            _aggregate.RemoveNull();
            return _aggregate;
        }

        public IAdjustmentBuilder Create()
        {
            _aggregate = new AdjustmentModel();
            return this;
        }

        public IAdjustmentBuilder Load(IAdjustmentKey adjustmentKey)
        {
            _aggregate = _adjustmentDal.GetData(adjustmentKey)
                         ?? throw new KeyNotFoundException("Adjustment tidak ditemukan");
            return this;
        }

        public IAdjustmentBuilder Brg(IBrgKey brgKey)
        {
            var brg = _brgDal.GetData(brgKey)
                ?? throw new KeyNotFoundException("Brg not found");
            _aggregate.BrgId = brg.BrgId;
            _aggregate.BrgCode = brg.BrgCode;
            _aggregate.BrgName = brg.BrgName;
            return this;
        }

        public IAdjustmentBuilder Warehouse(IWarehouseKey warehouseKey)
        {
            var warehouse = _warehouseDal.GetData(warehouseKey)
                ?? throw new KeyNotFoundException("Warehouse not found");
            _aggregate.WarehouseId = warehouse.WarehouseId;
            _aggregate.WarehouseName = warehouse.WarehouseName;
            return this;
        }

        public IAdjustmentBuilder Alasan(string alasan)
        {
            _aggregate.Alasan = alasan;
            return this;
        }

        public IAdjustmentBuilder AdjustmentDate(DateTime adjustmentDate)
        {
            _aggregate.AdjustmentDate = adjustmentDate;
            return this;
        }

        public IAdjustmentBuilder QtyAdjustInPcs(int qtyAdjustInPcs)
        {
            var brgKey = new BrgModel(_aggregate.BrgId);
            var stok = _stokBalanceWarehouseDal.ListData(brgKey)?.ToList() ??
                       new List<StokBalanceWarehouseModel>();
            var stokInPcs = stok.FirstOrDefault(x => x.WarehouseId == _aggregate.WarehouseId)?.Qty ?? 0;
            var brg = _brgBuilder.Load(brgKey).Build();
            var conversion = brg.ListSatuan?.DefaultIfEmpty(new BrgSatuanModel {Conversion = 1}).Max(x => x.Conversion) ?? 1;
            var stokBesar = conversion == 1 ? 0 : stokInPcs / conversion;
            var stokKecil = conversion == 1 ? stokInPcs : stokInPcs % conversion;
            
            _aggregate.QtyAwalBesar = stokBesar;
            _aggregate.QtyAwalKecil = stokKecil;
            _aggregate.QtyAwalInPcs = stokInPcs;
            
            _aggregate.QtyAdjustInPcs = qtyAdjustInPcs;
            _aggregate.QtyAdjustBesar = conversion == 1 ? 0 : qtyAdjustInPcs / conversion;
            _aggregate.QtyAdjustKecil = conversion == 1 ? qtyAdjustInPcs : qtyAdjustInPcs % conversion;
            
            _aggregate.QtyAkhirBesar = _aggregate.QtyAwalBesar + _aggregate.QtyAdjustBesar;
            _aggregate.QtyAkhirKecil = _aggregate.QtyAwalKecil + _aggregate.QtyAdjustKecil;
            _aggregate.QtyAkhirInPcs = _aggregate.QtyAwalInPcs + _aggregate.QtyAdjustInPcs;
            
            return this;
        }
    }
}