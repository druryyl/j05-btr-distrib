﻿using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.DriverAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.application.SupportContext.TglJamAgg;
using btr.domain.InventoryContext.DriverAgg;
using btr.domain.InventoryContext.PackingAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using btr.application.SupportContext.UserAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.domain.SupportContext.UserAgg;

namespace btr.application.InventoryContext.PackingAgg
{
    public interface IPackingBuilder : INunaBuilder<PackingModel>
    {
        IPackingBuilder Create();
        IPackingBuilder Load(IPackingKey packingKey);
        IPackingBuilder Attach(PackingModel model);
        IPackingBuilder User(IUserKey user);
        IPackingBuilder DeliveryDate(DateTime deliveryDate);

        IPackingBuilder Driver(IDriverKey driverKey);
        IPackingBuilder FilterFakturDate(Periode periode);
        IPackingBuilder KeywordSearch(string keyword);
        IPackingBuilder AddFaktur(IFakturKey fakturKey);
        IPackingBuilder RemoveFaktur(IFakturKey fakturKey);
        IPackingBuilder AddBrg<T>(T fakturSupplierBrg, int qtyBesar, string satBesar, 
            int qtyKecil, string satKecil, decimal hargaJual) 
            where T: IFakturKey, ISupplierKey, IBrgKey;

        IPackingBuilder RemoveBrg<T>(T fakturBrgKey)
            where T : IFakturKey, IBrgKey;
    }

    public class PackingBuilder : IPackingBuilder
    {
        private PackingModel _aggregate = new PackingModel();

        private readonly IPackingDal _packingDal;
        private readonly IPackingFakturDal _packingFakturDal;
        private readonly IPackingBrgDal _packingFakturSupplierDal;
        private readonly IFakturDal _fakturDal;
        private readonly IDriverDal _driverDal;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IUserDal _userDal;

        public PackingBuilder(IPackingDal packingDal,
            IPackingFakturDal packingFakturDal,
            IPackingBrgDal packingFakturSupplierDal,
            IDriverDal driverDal,
            IFakturDal fakturDal,
            IBrgBuilder brgBuilder, IUserDal userDal)
        {
            _packingDal = packingDal;
            _packingFakturDal = packingFakturDal;
            _packingFakturSupplierDal = packingFakturSupplierDal;
            _driverDal = driverDal;
            _fakturDal = fakturDal;
            _brgBuilder = brgBuilder;
            _userDal = userDal;
        }


        public PackingModel Build()
        {
            _aggregate.RemoveNull();
            return _aggregate;
        }

        public IPackingBuilder Create()
        {
            _aggregate = new PackingModel
            {
                PackingDate = DateTime.Now,
                DeliveryDate = DateTime.Now.AddDays(1),
                TglAwalFaktur = DateTime.Now.AddDays(-1),
                TglAkhirFaktur = DateTime.Now,
                ListBrg = new List<PackingBrgModel>(),
                ListFaktur = new List<PackingFakturModel>()
            };

            return this;
        }

        public IPackingBuilder Load(IPackingKey packingKey)
        {
            //      list detil
            _aggregate = _packingDal.GetData(packingKey)
                ?? throw new KeyNotFoundException("Packing Driver not found");
            _aggregate.ListFaktur = _packingFakturDal.ListData(_aggregate)?.ToList()
                ?? new List<PackingFakturModel>();
            _aggregate.ListBrg = _packingFakturSupplierDal.ListData(_aggregate)?.ToList()
                 ?? new List<PackingBrgModel>();
            
            return this;
        }

        public IPackingBuilder Attach(PackingModel model)
        {
            _aggregate = model;
            return this;
        }

        public IPackingBuilder User(IUserKey userKey)
        {
            var user = _userDal.GetData(userKey)
                       ?? throw new KeyNotFoundException("User not found");
            _aggregate.UserId = user.UserId;
            _aggregate.UserName = user.UserName;
            return this;
        }

        public IPackingBuilder DeliveryDate(DateTime deliveryDate)
        {
            _aggregate.DeliveryDate = deliveryDate;
            return this;
        }

        public IPackingBuilder Driver(IDriverKey driverKey)
        {
            var driver = _driverDal.GetData(driverKey)
                ?? throw new KeyNotFoundException("DriverId invalid");
            _aggregate.DriverId = driver.DriverId;
            _aggregate.DriverName = driver.DriverName;
            return this;
        }

        public IPackingBuilder FilterFakturDate(Periode periode)
        {
            _aggregate.TglAwalFaktur = periode.Tgl1;
            _aggregate.TglAkhirFaktur = periode.Tgl2;
            return this;
        }

        public IPackingBuilder KeywordSearch(string keyword)
        {
            _aggregate.KeywordSearch = keyword;
            return this;
        }

        public IPackingBuilder AddFaktur(IFakturKey fakturKey)
        {
            var noUrut = _aggregate.ListFaktur
                .DefaultIfEmpty(new PackingFakturModel { NoUrut = 0})
                .Max(x => x.NoUrut);
            noUrut++;
            var faktur = _fakturDal.GetData(fakturKey)
                ?? throw new KeyNotFoundException("FakturId invalid");
            _aggregate.ListFaktur.Add(new PackingFakturModel
            {
                NoUrut = noUrut,
                FakturId = faktur.FakturId,
                FakturCode = faktur.FakturCode,
                CustomerName = faktur.CustomerName,
                Address = faktur.Address,
                Kota = faktur.Kota,
                PackingId = _aggregate.PackingId,
                GrandTotal = faktur.GrandTotal,
            });
            return this;
        }

        public IPackingBuilder RemoveFaktur(IFakturKey fakturKey)
        {
            _aggregate.ListFaktur.RemoveAll(x => x.FakturId == fakturKey.FakturId);
            return this;
        }

        public IPackingBuilder AddBrg<T>(T key, 
            int qtyBesar, string satBesar, int qtyKecil, string satKecil,
            decimal hargaJual) where T : IFakturKey, ISupplierKey, IBrgKey
        {
            _aggregate.ListBrg
                .RemoveAll(x => 
                    x.FakturId == key.FakturId &&
                    x.BrgId == key.BrgId);
            var brg = _brgBuilder.Load(key).Build();
            _aggregate.ListBrg.Add(new PackingBrgModel
            {
                FakturId = key.FakturId,
                SupplierId = key.SupplierId,
                BrgId = key.BrgId,
                BrgName = brg.BrgName,
                BrgCode = brg.BrgCode,
                QtyBesar = qtyBesar,
                SatBesar = satBesar,
                QtyKecil = qtyKecil,
                SatKecil = satKecil,
                HargaJual = hargaJual
            });
            return this;
        }

        public IPackingBuilder RemoveBrg<T>(T key) where T : IFakturKey, IBrgKey
        {
            _aggregate.ListBrg
                .RemoveAll(x => 
                    x.FakturId == key.FakturId &&
                    x.BrgId == key.BrgId);
            return this;
        }
    }
}
