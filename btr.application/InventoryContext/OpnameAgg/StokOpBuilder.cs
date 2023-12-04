using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.OpnameAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.domain.InventoryContext.StokBalanceAgg;
using btr.nuna.Domain;

namespace btr.application.InventoryContext.OpnameAgg
{
    public interface IStokOpBuilder : INunaBuilder<StokOpModel>
    {
        IStokOpBuilder Create<T>(T brgWarehouse) where T: IBrgKey, IWarehouseKey;
        IStokOpBuilder Load(IStokOpKey stokOpKey);
        IStokOpBuilder Periode(DateTime dateTime);
        IStokOpBuilder QtyOpname(int qtyBesar, int qtyKecil);
        IStokOpBuilder User(IUserKey userKey);
    }
    public class StokOpBuilder : IStokOpBuilder
    {
        private StokOpModel _aggregate = new StokOpModel();
        
        private readonly IStokOpDal _stokOpDal;
        private readonly IBrgDal _brgDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IStokBalanceWarehouseDal _stokBalanceWarehouseDal;

        public StokOpBuilder(IStokOpDal stokOpDal, 
            IBrgDal brgDal, 
            IWarehouseDal warehouseDal, 
            IStokBalanceWarehouseDal stokBalanceWarehouseDal)
        {
            _stokOpDal = stokOpDal;
            _brgDal = brgDal;
            _warehouseDal = warehouseDal;
            _stokBalanceWarehouseDal = stokBalanceWarehouseDal;
        }

        public StokOpModel Build()
        {
            _aggregate.RemoveNull();
            CalculateAdjust();
            return _aggregate;

            //  local function
            void CalculateAdjust()
            {
                var stokBalance = _stokBalanceWarehouseDal.ListData(_aggregate)
                    ?? new List<StokBalanceWarehouseModel>();
                var thisStok = stokBalance.FirstOrDefault(x => x.WarehouseId == _aggregate.WarehouseId)
                    ?? new StokBalanceWarehouseModel{Qty = 0};
                var qtyAdjust = _aggregate.QtyPcsOpname - thisStok.Qty;
                var conversion = GetConversion(_aggregate);
                var qtyBesarAdjust = (int)Math.Floor((decimal)qtyAdjust / conversion);
                var qtyKecilAdjust = qtyAdjust - (qtyBesarAdjust * conversion);
                
                _aggregate.QtyBesarAdjust = qtyBesarAdjust;
                _aggregate.QtyKecilAdjust = qtyKecilAdjust;
                _aggregate.QtyPcsAdjust = qtyAdjust;
                
                _aggregate.QtyBesarAwal = (int)Math.Floor((decimal)thisStok.Qty / conversion);
                _aggregate.QtyKecilAwal = thisStok.Qty - (_aggregate.QtyBesarAwal * conversion);
                _aggregate.QtyPcsAwal = thisStok.Qty;
            }
        }

        public IStokOpBuilder Create<T>(T brgLayanan) where T : IBrgKey, IWarehouseKey
        {
            _aggregate = new StokOpModel();

            var brg = _brgDal.GetData(brgLayanan)
                ?? throw new KeyNotFoundException($"BrgKey {brgLayanan.BrgId} not found");
            _aggregate.BrgId = brg.BrgId;
            _aggregate.BrgName = brg.BrgName;
            
            var warehouse = _warehouseDal.GetData(brgLayanan)
                ?? throw new KeyNotFoundException($"WarehouseKey {brgLayanan.WarehouseId} not found");
            _aggregate.WarehouseId = warehouse.WarehouseId;
            _aggregate.WarehouseName = warehouse.WarehouseName;

            return this;
        }

        public IStokOpBuilder Load(IStokOpKey stokOpKey)
        {
            _aggregate = _stokOpDal.GetData(stokOpKey)
                ?? throw new KeyNotFoundException("StokOpKey not found ");
            return this;
        }

        public IStokOpBuilder Periode(DateTime dateTime)
        {
            _aggregate.PeriodeOp = dateTime.Date;
            return this;
        }

        private int GetConversion(IBrgKey brgKey)
        {
            var brg = _brgDal.GetData(brgKey)
                ?? new BrgModel();
            var conversion = brg.ListSatuan
                .DefaultIfEmpty(new BrgSatuanModel{Conversion = 1})
                .Max(x => x.Conversion);
            return conversion;
        }
        public IStokOpBuilder QtyOpname(int qtyBesar, int qtyKecil)
        {
            var conversion = GetConversion(_aggregate);
            
            //  convert ke satuan terbesar jika satuan kecil lebih dari conversion
            var additionalQtyBesar = qtyKecil > conversion 
                ? Math.Floor((decimal)qtyKecil / conversion) : 0;
            var sisaQtyKecil = qtyKecil - (additionalQtyBesar * conversion);
            if (conversion == 1)
            {
                additionalQtyBesar = 0;
                sisaQtyKecil = qtyKecil;
            }
            
            _aggregate.QtyBesarOpname = qtyBesar + (int)additionalQtyBesar;
            _aggregate.QtyKecilOpname = (int)sisaQtyKecil;
            _aggregate.QtyPcsOpname = (qtyBesar * conversion) + qtyKecil;
            return this;
        }

        public IStokOpBuilder User(IUserKey userKey)
        {
            _aggregate.UserId = userKey.UserId;
            return this;
        }
    }
}
