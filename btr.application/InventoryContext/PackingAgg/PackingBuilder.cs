using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.DriverAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.application.SupportContext.TglJamAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.DriverAgg;
using btr.domain.InventoryContext.PackingAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.PackingAgg
{
    public interface IPackingBuilder : INunaBuilder<PackingModel>
    {
        IPackingBuilder LoadOrCreate(IDriverKey driverKey, DateTime DeliveryDate);
        IPackingBuilder Load(IPackingKey packingKey);
        IPackingBuilder Load(IDriverKey driverKey, DateTime DeliveryDate);
        IPackingBuilder Attach(PackingModel model);

        IPackingBuilder Warehouse(IWarehouseKey wareouseKey);

        IPackingBuilder Driver(IDriverKey driverKey);
        IPackingBuilder Route(string route);
        IPackingBuilder AddFaktur(IFakturKey fakturKey);
        IPackingBuilder RemoveFaktur(IFakturKey fakturKey);
        IPackingBuilder GenSupplier();
    }

    public class PackingBuilder : IPackingBuilder
    {
        private PackingModel _aggregate = new PackingModel();

        private readonly IPackingDal _packingDal;
        private readonly IPackingFakturDal _packingFakturDal;
        private readonly IPackingBrgDal _packingSupplierDal;
        private readonly IFakturDal _fakturDal;
        private readonly IDriverDal _driverDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly ITglJamDal _dateTime;
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IBrgBuilder _brgBuilder;

        public PackingBuilder(IPackingDal packingDal,
            IPackingFakturDal packingFakturDal,
            IPackingBrgDal packingSupplierDal,
            IDriverDal driverDal,
            IWarehouseDal warehouseDal,
            ITglJamDal dateTime,
            IFakturDal fakturDal,
            IFakturBuilder fakturBuilder,
            IBrgBuilder brgBuilder)
        {
            _packingDal = packingDal;
            _packingFakturDal = packingFakturDal;
            _packingSupplierDal = packingSupplierDal;
            _driverDal = driverDal;
            _warehouseDal = warehouseDal;
            _dateTime = dateTime;
            _fakturDal = fakturDal;
            _fakturBuilder = fakturBuilder;
            _brgBuilder = brgBuilder;
        }


        public PackingModel Build()
        {
            _aggregate.RemoveNull();
            return _aggregate;
        }

        public IPackingBuilder LoadOrCreate(IDriverKey driverKey, DateTime deliveryDate)
        {
            //      get header data
            var listPacking = _packingDal.ListData(new Periode(deliveryDate))
                ?? new List<PackingModel>();
            _aggregate = listPacking.FirstOrDefault(x => x.DriverId == driverKey.DriverId);

            if (_aggregate == null)
                Create_(driverKey, deliveryDate);
            else
                Load(_aggregate);

            return this;
        }

        private void Create_(IDriverKey driverKey, DateTime deliveryDate) 
        {
            _aggregate = new PackingModel
            {
                PackingDate = _dateTime.Now(),
                DeliveryDate = deliveryDate,
            };
            _aggregate = Attach(_aggregate)
                .Driver(driverKey)
                .Build();

            //      list detil
            _aggregate.ListFaktur = new List<PackingFakturModel>();
            _aggregate.ListSupplier = new List<PackingSupplierModel>();
        }

        public IPackingBuilder Load(IPackingKey packingKey)
        {
            //      list detil
            _aggregate = _packingDal.GetData(packingKey)
                ?? throw new KeyNotFoundException("Packing Driver not found");
            _aggregate.ListFaktur = _packingFakturDal.ListData(_aggregate)?.ToList()
                ?? new List<PackingFakturModel>();
            var listSupBrg = _packingSupplierDal.ListData(_aggregate)?.ToList()
                ?? new List<PackingBrgModel>();
            //      projection detil per-brg
            _aggregate.ListSupplier = (
                from c in listSupBrg
                group c by new { c.PackingId, c.SupplierId, c.SupplierName } into g
                select new PackingSupplierModel
                {
                    PackingId = g.Key.PackingId,
                    SupplierId = g.Key.SupplierId,
                    SupplierName = g.Key.SupplierName,
                    ListBrg = g.Adapt<List<PackingBrgModel>>()
                }).ToList();

            return this;
        }

        public IPackingBuilder Load(IDriverKey driverKey, DateTime deliveryDate)
        {
            var listPacking = _packingDal.ListData(new Periode(deliveryDate))?.ToList()
                ?? throw new KeyNotFoundException("Packing not found");
            var packing = listPacking.FirstOrDefault(x => x.DriverId == driverKey.DriverId)
                ?? throw new KeyNotFoundException("Packing not found");
            return Load(packing);
        }

        public IPackingBuilder Attach(PackingModel model)
        {
            _aggregate = model;
            return this;
        }

        public IPackingBuilder Warehouse(IWarehouseKey wareouseKey)
        {
            var warehouse = _warehouseDal.GetData(wareouseKey)
                ?? throw new KeyNotFoundException("WarehouseId invalid");
            _aggregate.WarehouseId = warehouse.WarehouseId;
            _aggregate.WarehouseName = warehouse.WarehouseName;
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

        public IPackingBuilder Route(string route)
        {
            _aggregate.Route = route;
            return this;
        }

        public IPackingBuilder AddFaktur(IFakturKey fakturKey)
        {
            var noUrut = _aggregate.ListFaktur.DefaultIfEmpty(new PackingFakturModel { NoUrut = 0}).Max(x => x.NoUrut);
            noUrut++;
            var faktur = _fakturDal.GetData(fakturKey)
                ?? throw new KeyNotFoundException("FakturId invalid");
            _aggregate.ListFaktur.Add(new PackingFakturModel
            {
                FakturId = faktur.FakturId,
                FakturCode = faktur.FakturCode,
                CustomerName = faktur.CustomerName,
                Address = faktur.Address,
                Kota = faktur.Kota,
                PackingId = _aggregate.PackingId,
                GrandTotal = faktur.GrandTotal,
            });
            AddBrgFaktur(fakturKey);
            return this;
        }

        public IPackingBuilder RemoveFaktur(IFakturKey fakturKey)
        {
            _aggregate.ListFaktur.RemoveAll(x => x.FakturId == fakturKey.FakturId);
            RemoveBrgFaktur(fakturKey);
            //  TODO: RemoveBrg
            return this;
        }

        private void AddBrgFaktur(IFakturKey fakturKey)
        {
            var faktur = _fakturBuilder.Load(fakturKey).Build();

            var listSupBrg = _aggregate.ListSupplier
                .SelectMany(hdr => hdr.ListBrg, (hdr, dtl) => dtl)?.ToList()
                ?? new List<PackingBrgModel>();
            foreach (var item in faktur.ListItem)
            {
                var supBrg = listSupBrg.FirstOrDefault(x => x.BrgId == item.BrgId);
                if (supBrg is null)
                {
                    supBrg = CreateNewSupBrg(item);
                    listSupBrg.Add(supBrg);
                }

                var allPcs = (supBrg.QtyBesar * item.Conversion) + supBrg.QtyKecil + item.QtyPotStok;
                if (item.Conversion == 0)
                {
                    supBrg.QtyKecil = allPcs;
                    supBrg.QtyBesar = 0;
                }
                else
                {
                    supBrg.QtyBesar = (int)(allPcs / item.Conversion);
                    supBrg.QtyKecil = allPcs % item.Conversion;
                }
                supBrg.HargaJual = allPcs * item.HrgSatKecil;
            }

            _aggregate.ListSupplier = (
                from c in listSupBrg
                group c by new { c.PackingId, c.SupplierId, c.SupplierName } into g
                select new PackingSupplierModel
                {
                    PackingId = g.Key.PackingId,
                    SupplierId = g.Key.SupplierId,
                    SupplierName = g.Key.SupplierName,
                    ListBrg = g.Adapt<List<PackingBrgModel>>()
                }).ToList();

            #region INNER-HELPER
            PackingBrgModel CreateNewSupBrg(FakturItemModel fakturItem)
            {
                var brg = _brgBuilder.Load(fakturItem).Build();
                var result = new PackingBrgModel
                {
                    BrgId = brg.BrgId,
                    BrgName = brg.BrgName,
                    SupplierId = brg.SupplierId,
                    SupplierName = brg.SupplierName,
                    HargaJual = 0,
                    QtyBesar = 0,
                    QtyKecil = 0,
                    SatuanBesar = brg.ListSatuan.OrderBy(x => x.Conversion).Last().Satuan,
                    SatuanKecil = brg.ListSatuan.OrderBy(x => x.Conversion).First().Satuan,
                };
                return result;

            }
            #endregion
        }

        private void RemoveBrgFaktur(IFakturKey fakturKey)
        {
            var faktur = _fakturBuilder.Load(fakturKey).Build();

            var listSupBrg = _aggregate.ListSupplier
                .SelectMany(hdr => hdr.ListBrg, (hdr, dtl) => dtl)?.ToList()
                ?? new List<PackingBrgModel>();

            foreach (var item in faktur.ListItem)
            {
                var supBrg = listSupBrg.FirstOrDefault(x => x.BrgId == item.BrgId);
                if (supBrg is null)
                    continue;

                var allPcs = (supBrg.QtyBesar * item.Conversion) + supBrg.QtyKecil - item.QtyPotStok;
                if (item.Conversion == 0)
                {
                    supBrg.QtyKecil = allPcs;
                    supBrg.QtyBesar = 0;
                }
                else
                {
                    supBrg.QtyBesar = (int)(allPcs / item.Conversion);
                    supBrg.QtyKecil = allPcs % item.Conversion;
                }
                supBrg.HargaJual = allPcs * item.HrgSatKecil;
            }

            _aggregate.ListSupplier = (
                from c in listSupBrg
                group c by new { c.PackingId, c.SupplierId, c.SupplierName } into g
                select new PackingSupplierModel
                {
                    PackingId = g.Key.PackingId,
                    SupplierId = g.Key.SupplierId,
                    SupplierName = g.Key.SupplierName,
                    ListBrg = g.Adapt<List<PackingBrgModel>>()
                }).ToList();
        }

        public IPackingBuilder GenSupplier()
        {
            ////      collect all faktur;
            //var listFakturItemAll = new List<FakturItemModel>();
            //foreach(var item in _aggregate.ListFaktur)
            //{
            //    var faktur = _fakturBuilder.Load(item).Build();
            //    listFakturItemAll.AddRange(faktur.ListItem);
            //}

            //var listSupBrg = new List<PackingBrgModel>();
            //foreach (var item in listFakturItemAll)
            //{
            //    var supBrg = listSupBrg.FirstOrDefault(x => x.BrgId == item.BrgId);
            //    if (supBrg is null)
            //    {
            //        supBrg = CreateNewSupBrg(item);
            //        listSupBrg.Add(supBrg);
            //    }
            //    var inPcs = item.ListQtyHarga.Sum(x => x.Qty * x.Conversion);
            //    var conversion = item.ListQtyHarga.Max(x => x.Conversion);
            //    var qtyBesar = Math.Floor((decimal)inPcs / conversion);
            //    var qtyKecil = inPcs % conversion;
            //    supBrg.QtyBesar += (int)qtyBesar;
            //    supBrg.QtyKecil += qtyKecil;
            //    supBrg.HargaJual += item.Total;
            //}

            //_aggregate.ListSupplier = (
            //    from c in listSupBrg
            //    group c by new { c.PackingId, c.SupplierId, c.SupplierName } into g
            //    select new PackingSupplierModel
            //    {
            //        PackingId = g.Key.PackingId,
            //        SupplierId = g.Key.SupplierId,
            //        SupplierName = g.Key.SupplierName,
            //        ListBrg = g.Adapt<List<PackingBrgModel>>()
            //    }).ToList();

            //return this;

            //#region INNER-HELPER
            //PackingBrgModel CreateNewSupBrg(FakturItemModel fakturItem)
            //{
            //    var brg = _brgBuilder.Load(fakturItem).Build();
            //    var result = new PackingBrgModel
            //    {
            //        BrgId = brg.BrgId,
            //        BrgName = brg.BrgName,
            //        SupplierId = brg.SupplierId,
            //        SupplierName = brg.SupplierName,
            //        HargaJual = 0,
            //        QtyBesar = 0,
            //        QtyKecil = 0,
            //        SatuanBesar = brg.ListSatuan.OrderBy(x => x.Conversion).Last().Satuan,
            //        SatuanKecil = brg.ListSatuan.OrderBy(x => x.Conversion).First().Satuan,
            //    };
            //    return result;

            //}
            //#endregion
            throw new NotImplementedException();
        }

    }
}
