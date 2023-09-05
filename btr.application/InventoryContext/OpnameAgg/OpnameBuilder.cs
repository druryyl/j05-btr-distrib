using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.OpnameAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.InventoryContext.OpnameAgg
{
    public interface IOpnameBuilder : INunaBuilder<OpnameModel>
    {
        IOpnameBuilder Load(IOpnameKey opnameKey);
        IOpnameBuilder Create();

        IOpnameBuilder Brg(IBrgKey brgKey);
        IOpnameBuilder Warehouse(IWarehouseKey warehouseKey);

        IOpnameBuilder QtyAwal(int qtyBesar, int qtyKecil);
        IOpnameBuilder QtyOpname(int qtyBesar, int qtyKecil);
        IOpnameBuilder Nilai(decimal nilai);
    }
    
    public class OpnameBuilder : IOpnameBuilder
    {
        private OpnameModel _agg;
        private readonly IOpnameDal _opnameDal;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IWarehouseDal _warehouseDal;

        public OpnameBuilder(IOpnameDal opnameDal, 
            IBrgBuilder brgBuilder, 
            IWarehouseDal warehouseDal)
        {
            _opnameDal = opnameDal;
            _brgBuilder = brgBuilder;
            _warehouseDal = warehouseDal;
        }

        public OpnameModel Build()
        {
            _agg.RemoveNull();
            return _agg;
        }

        public IOpnameBuilder Load(IOpnameKey opnameKey)
        {
            _agg = _opnameDal.GetData(opnameKey)
                   ?? throw new KeyNotFoundException("OpnameId invalid");
            return this;
        }

        public IOpnameBuilder Create()
        {
            _agg = new OpnameModel();
            return this;
        }

        public IOpnameBuilder Brg(IBrgKey brgKey)
        {
            var brg = _brgBuilder.Load(brgKey).Build();
            var satKecil = brg.ListSatuan
                .FirstOrDefault(x => x.Conversion == 1);
            var satBesar = brg.ListSatuan
                .Where(x => x.Conversion > 1)
                .OrderBy(x => x.Conversion)
                .FirstOrDefault();       
            
            _agg.BrgId = brg.BrgId;
            _agg.BrgCode = brg.BrgCode;
            _agg.BrgName = brg.BrgName;
            _agg.Satuan1 = satKecil?.Satuan ?? string.Empty;
            _agg.Conversion1 = 1;
            _agg.Satuan2 = satBesar?.Satuan ?? string.Empty;
            _agg.Conversion2 = satBesar?.Conversion ?? 1;
            return this;
        }

        public IOpnameBuilder Warehouse(IWarehouseKey warehouseKey)
        {
            var warehouse = _warehouseDal.GetData(warehouseKey)
                            ?? throw new KeyNotFoundException("WarehouseId not found");
            _agg.WarehouseId = warehouse.WarehouseId;
            _agg.WarehouseName = warehouse.WarehouseName;
            return this;
        }

        public IOpnameBuilder QtyAwal(int qtyBesar, int qtyKecil)
        {
            _agg.Qty1Awal = qtyKecil;
            _agg.Qty2Awal = qtyBesar;

            SetQtyAdjust();
            return this;
        }

        public IOpnameBuilder QtyOpname(int qtyBesar, int qtyKecil)
        {
            _agg.Qty1Opname = qtyKecil;
            _agg.Qty2Opname = qtyBesar;
            SetQtyAdjust();
            return this;
        }

        private void SetQtyAdjust()
        {
            _agg.Qty1Adjust = _agg.Qty1Opname - _agg.Qty1Awal;
            _agg.Qty2Adjust = _agg.Qty2Opname - _agg.Qty2Awal;
        }
        public IOpnameBuilder Nilai(decimal nilai)
        {
            _agg.Nilai = nilai;
            return this;
        }
    }
}