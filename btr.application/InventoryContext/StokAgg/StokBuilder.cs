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
            int qty, decimal nilai, string reffId, string jenisMutasi, string keterangan, DateTime mutasiDate);
        IStokBuilder Load(IStokKey stokKey);
        IStokBuilder Attach(StokModel stok);
        IStokBuilder RemoveStok(int qty, decimal hargaJual, string reffId, string jenisMutasi, string keterangan, DateTime mutasiDate);
        IStokBuilder RollBack(IReffKey reffKey);
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
            int qty, decimal nilai, string reffId, string jenisMutasi, string keterangan, DateTime mutasiDate)
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
                StokDate = mutasiDate,
                NilaiPersediaan = nilai,
                ListMutasi = new List<StokMutasiModel>()
            };
            var newMutasi = new StokMutasiModel
            {
                BrgId = brgKey.BrgId,
                WarehouseId = warehouseKey.WarehouseId,
                ReffId = reffId,
                JenisMutasi = jenisMutasi,
                NoUrut = 0,
                MutasiDate = mutasiDate,
                PencatatanDate = _dateTime.Now,
                QtyIn = qty,
                QtyOut = 0,
                HargaJual = 0,
                Keterangan = keterangan
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

        public IStokBuilder RemoveStok(int qty, decimal hargaJual, string reffId, string jenisMutasi, string keterangan, DateTime mutasiDate)
        {
            if (_agg.Qty < qty)
                throw new ArgumentException("Stok tidak mencukupi");
            var noUrut = _agg.ListMutasi.DefaultIfEmpty(new StokMutasiModel { NoUrut = 0}).Max(x => x.NoUrut);
            noUrut++;
            var newMutasi = new StokMutasiModel
            {
                QtyIn = 0,
                QtyOut = qty,
                HargaJual = hargaJual,
                ReffId = reffId,
                BrgId = _agg.BrgId,
                WarehouseId = _agg.WarehouseId,
                MutasiDate = mutasiDate,
                PencatatanDate = _dateTime.Now,
                JenisMutasi = jenisMutasi,
                NoUrut = noUrut,
                Keterangan = keterangan
            };
            _agg.Qty -= qty;
            _agg.ListMutasi.Add(newMutasi);
            return this;
        }

        public IStokBuilder RollBack(IReffKey reffKey)
        {
            _agg.ListMutasi.RemoveAll(x => x.ReffId == reffKey.ReffId);
            _agg.Qty = _agg.ListMutasi.Sum(x => x.QtyIn -  x.QtyOut);
            return this;
        }

        public IStokBuilder Attach(StokModel stok)
        {
            _agg = stok;
            return this;
        }
    }
}