using System;
using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.InventoryContext.StokAgg
{
    public interface IStokBuilder : INunaBuilder<StokModel>
    {
        IStokBuilder Create(IBrgKey brgKey, IWarehouseKey warehouseKey, 
            int qty, decimal nilai, string reffId, string jenisMutasi);
        IStokBuilder Load(IStokKey stokKey);
        IStokBuilder RemoveStok(int qty, decimal hargaJual, string reffId, string jenisMutasi);
    }
    
    public class StokBuilder : IStokBuilder
    {
        private StokModel _agg;
        private readonly IBrgDal _brgDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly DateTimeProvider _dateTime;
        private readonly IStokDal _stokDal;
        private readonly IStokMutasiDal _stokMutasiDal;

        public StokBuilder(IBrgDal brgDal, 
            IWarehouseDal warehouseDal, 
            DateTimeProvider dateTime, 
            IStokDal stokDal, 
            IStokMutasiDal stokMutasiDal)
        {
            _brgDal = brgDal;
            _warehouseDal = warehouseDal;
            _dateTime = dateTime;
            _stokDal = stokDal;
            _stokMutasiDal = stokMutasiDal;
        }

        public StokModel Build()
        {
            _agg.RemoveNull();
            return _agg;
        }

        public IStokBuilder Create(IBrgKey brgKey, IWarehouseKey warehouseKey, 
            int qty, decimal nilai, string reffId, string jenisMutasi)
        {
            var brg = _brgDal.GetData(brgKey)
                ?? throw new KeyNotFoundException($"[Create-Stok] BrgId invalid ({brgKey.BrgId})");
            var warehouse = _warehouseDal.GetData(warehouseKey)
                ?? throw new KeyNotFoundException($"[Create-Stok] WarehouseId invalid ({warehouseKey.WarehouseId})");

            _agg = new StokModel
            {
                BrgId = brgKey.BrgId,
                BrgName = brg.BrgName,
                WarehouseId = warehouseKey.WarehouseId,
                WarehouseName = warehouse.WarehouseName,
                Qty = qty,
                QtyIn = qty,
                ReffId = reffId,
                StokDate = _dateTime.Now,
                NilaiPersediaan = nilai,
                ListMutasi = new List<StokMutasiModel>()
            };
            var newMutasi = new StokMutasiModel
            {
                ReffId = reffId,
                JenisMutasi = jenisMutasi,
                NoUrut = 0,
                MutasiDate = _dateTime.Now,
                QtyIn = qty,
                QtyOut = 0,
                HargaJual = 0
            };
            _agg.ListMutasi.Add(newMutasi);
            return this;
        }

        public IStokBuilder Load(IStokKey stokKey)
        {
            _agg = _stokDal.GetData(stokKey)
                   ?? throw new KeyNotFoundException("StokId not found");
            _agg.ListMutasi = _stokMutasiDal.ListData(stokKey)?.ToList() 
                    ?? new List<StokMutasiModel>();
            return this;
        }

        public IStokBuilder RemoveStok(int qty, decimal hargaJual, string reffId, string jenisMutasi)
        {
            if (_agg.Qty < qty)
                throw new ArgumentException("Stok tidak mencukupi");
            var noUrut = _agg.ListMutasi.Count;
            var newMutasi = new StokMutasiModel
            {
                QtyIn = 0,
                QtyOut = qty,
                HargaJual = hargaJual,
                ReffId = reffId,
                MutasiDate = _dateTime.Now,
                JenisMutasi = jenisMutasi,
                NoUrut = noUrut,
            };
            _agg.Qty -= qty;
            _agg.ListMutasi.Add(newMutasi);
            return this;
        }
    }
}