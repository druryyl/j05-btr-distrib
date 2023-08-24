using System;
using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.WarehouseAgg.Contracts;
using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.SalesContext.FakturAgg.Workers
{
    public interface IFakturBuilder : INunaBuilder<FakturModel>
    {
        IFakturBuilder CreateNew(IUserKey userKey);
        IFakturBuilder Load(IFakturKey fakturKey);
        IFakturBuilder Attach(FakturModel faktur);
        IFakturBuilder FakturDate(DateTime fakturDate);
        IFakturBuilder Customer(ICustomerKey customerKey);
        IFakturBuilder SalesPerson(ISalesPersonKey salesPersonKey);
        IFakturBuilder Warehouse(IWarehouseKey warehouseKey);
        IFakturBuilder TglRencanaKirim(DateTime tglRencanaKirim);
        IFakturBuilder AddItem(IBrgKey brgKey, string qtyString, string discountString, decimal ppnProsen);
        IFakturBuilder ClearItem();
        IFakturBuilder CalcTotal();
    }

    public class FakturBuilder : IFakturBuilder
    {
        private FakturModel _aggRoot = new FakturModel();
        private readonly IFakturDal _fakturDal;
        private readonly IFakturItemDal _fakturItemDal;
        private readonly IFakturQtyHargaDal _fakturQtyHargaDal;
        private readonly IFakturDiscountDal _fakturDiscountDal;

        private readonly ICustomerDal _customerDal;
        private readonly ISalesPersonDal _salesPersonDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IBrgBuilder _brgBuilder;
        private readonly DateTimeProvider _dateTime;

        public FakturBuilder(IFakturDal fakturDal,
            IFakturItemDal fakturItemDal,
            IFakturQtyHargaDal fakturQtyHargaDal,
            IFakturDiscountDal fakturDiscountDal,
            ICustomerDal customerDal,
            ISalesPersonDal salesPersonDal,
            IWarehouseDal warehouseDal,
            IBrgBuilder brgBuilder,
            DateTimeProvider dateTime)
        {
            _fakturDal = fakturDal;
            _fakturItemDal = fakturItemDal;
            _fakturQtyHargaDal = fakturQtyHargaDal;
            _fakturDiscountDal = fakturDiscountDal;

            _customerDal = customerDal;
            _salesPersonDal = salesPersonDal;
            _warehouseDal = warehouseDal;
            _brgBuilder = brgBuilder;
            _dateTime = dateTime;
        }

        public FakturModel Build()
        {
            _aggRoot.RemoveNull();
            return _aggRoot;
        }

        public IFakturBuilder CreateNew(IUserKey userKey)
        {
            _aggRoot = new FakturModel
            {
                CreateTime = _dateTime.Now,
                LastUpdate = new DateTime(3000, 1, 1),
                UserId = userKey.UserId,
                ListItem = new List<FakturItemModel>()
            };
            return this;
        }

        public IFakturBuilder Load(IFakturKey fakturKey)
        {
            _aggRoot = _fakturDal.GetData(fakturKey)
                       ?? throw new KeyNotFoundException($"Faktur nof found ({fakturKey.FakturId})");
            _aggRoot.ListItem = _fakturItemDal.ListData(fakturKey)?.ToList()
                                ?? new List<FakturItemModel>();
            

            var allQtyHarga = _fakturQtyHargaDal.ListData(fakturKey)?.ToList()
                              ?? new List<FakturQtyHargaModel>();
            var allDiscount = _fakturDiscountDal.ListData(fakturKey)?.ToList()
                              ?? new List<FakturDiscountModel>();

            foreach (var item in _aggRoot.ListItem)
            {
                item.ListQtyHarga = allQtyHarga.Where(x => x.FakturItemId == item.FakturItemId).ToList();
                item.ListDiscount = allDiscount.Where(x => x.FakturItemId == item.FakturItemId).ToList();
            }

            return this;
        }

        public IFakturBuilder Attach(FakturModel faktur)
        {
            _aggRoot = faktur;
            return this;
        }

        public IFakturBuilder FakturDate(DateTime fakturDate)
        {
            _aggRoot.FakturDate = fakturDate;
            return this;
        }

        public IFakturBuilder Customer(ICustomerKey customerKey)
        {
            var customer = _customerDal.GetData(customerKey)
                           ?? throw new KeyNotFoundException($"CustomerId not found ({customerKey.CustomerId})");
            _aggRoot.CustomerId = customer.CustomerId;
            _aggRoot.CustomerName = customer.CustomerName;
            _aggRoot.Plafond = customer.Plafond;
            _aggRoot.CreditBalance = customer.CreditBalance;
            return this;
        }

        public IFakturBuilder SalesPerson(ISalesPersonKey salesPersonKey)
        {
            var salesPerson = _salesPersonDal.GetData(salesPersonKey)
                              ?? throw new KeyNotFoundException(
                                  $"SalesPersonId not found ({salesPersonKey.SalesPersonId})");
            _aggRoot.SalesPersonId = salesPerson.SalesPersonId;
            _aggRoot.SalesPersonName = salesPerson.SalesPersonName;
            return this;
        }

        public IFakturBuilder Warehouse(IWarehouseKey warehouseKey)
        {
            var warehouse = _warehouseDal.GetData(warehouseKey)
                            ?? throw new KeyNotFoundException($"WarehouseId not found ({warehouseKey.WarehouseId})");
            _aggRoot.WarehouseId = warehouse.WarehouseId;
            _aggRoot.WarehouseName = warehouse.WarehouseName;
            return this;
        }

        public IFakturBuilder TglRencanaKirim(DateTime tglRencanaKirim)
        {
            _aggRoot.TglRencanaKirim = tglRencanaKirim;
            return this;
        }

        public IFakturBuilder AddItem(IBrgKey brgKey, string qtyString,
            string discountString, decimal ppnProsen)
        {
            var noUrutMax = _aggRoot.ListItem
                .DefaultIfEmpty(new FakturItemModel() { NoUrut = 0 })
                .Max(x => x.NoUrut);
            var noUrut = noUrutMax + 1;

            var brg = _brgBuilder.Load(brgKey).Build();
            var newItem = new FakturItemModel
            {
                BrgId = brgKey.BrgId,
                BrgName = brg.BrgName,
                NoUrut = noUrut,
                ListQtyHarga = GenListStokHarga(brg, qtyString).ToList(),
            };
            newItem.Qty = newItem.ListQtyHarga.Sum(x => x.Qty * x.Conversion);
            newItem.SubTotal = newItem.ListQtyHarga.Sum(x => x.Qty * x.HargaJual);

            newItem.ListDiscount = GenListDiscount(brgKey.BrgId, newItem.SubTotal, discountString).ToList();
            newItem.DiscountRp = newItem.ListDiscount.Sum(x => x.DiscountRp);
            newItem.PpnProsen = 11;
            newItem.PpnRp = (newItem.SubTotal - newItem.DiscountRp) * 0.11;
            newItem.Total = newItem.SubTotal - newItem.DiscountRp + newItem.PpnRp;

            _aggRoot.ListItem.Add(newItem);
            return this;
        }

        private static IEnumerable<FakturQtyHargaModel> GenListStokHarga(BrgModel brg, string qtyString)
        {
            //  TODO: Perbaiki GenList Stok-Harga di Faktur Builder
            var result = new List<FakturQtyHargaModel>();
            // var qtys = ParseStringMultiNumber(qtyString, 3);
            // var satuanBesar = brg.ListSatuanHarga.OrderBy(x => x.Conversion).Last();
            // var satuanKecil = brg.ListSatuanHarga.OrderBy(x => x.Conversion).First();
            // var hrgBesar = brg.ListSatuanHarga.FirstOrDefault(x => x.Satuan == satuanBesar.Satuan)?.HargaJual ?? 0;
            // var hrgKecil = brg.ListSatuanHarga.FirstOrDefault(x => x.Satuan == satuanKecil.Satuan)?.HargaJual ?? 0;
            //
            // result.Add(new FakturQtyHargaModel(1, brg.BrgId, satuanBesar.Satuan,
            //     satuanBesar.Conversion, (int)qtys[0], hrgBesar));
            // result.Add(new FakturQtyHargaModel(2, brg.BrgId, satuanKecil.Satuan,
            //     satuanKecil.Conversion, (int)qtys[1], hrgKecil));
            // result.Add(new FakturQtyHargaModel(3, brg.BrgId, satuanKecil.Satuan,
            //     satuanKecil.Conversion, (int)qtys[2], 0));
            // result.RemoveAll(x => x.Qty == 0);

            return result;
        }

        private static IEnumerable<FakturDiscountModel> GenListDiscount(string brgId, double subTotal,
            string disccountString)
        {
            var discs = ParseStringMultiNumber(disccountString, 4);

            var discRp = new double[4];
            discRp[0] = subTotal * discs[0] / 100;
            var newSubTotal = subTotal - discRp[0];
            discRp[1] = newSubTotal * discs[1] / 100;
            newSubTotal -= discRp[1];
            discRp[2] = newSubTotal * discs[2] / 100;
            newSubTotal -= discRp[2];
            discRp[3] = newSubTotal * discs[3] / 100;

            var result = new List<FakturDiscountModel>
            {
                new FakturDiscountModel(1, brgId, discs[0], discRp[0]),
                new FakturDiscountModel(2, brgId, discs[1], discRp[1]),
                new FakturDiscountModel(3, brgId, discs[2], discRp[2]),
                new FakturDiscountModel(4, brgId, discs[3], discRp[3])
            };
            result.RemoveAll(x => x.DiscountProsen == 0);
            return result;
        }

        private static List<double> ParseStringMultiNumber(string str, int size)
        {
            var result = new List<double>();
            for (var i = 0; i < size; i++)
                result.Add(0);

            var resultStr = (str == string.Empty ? "0" : str).Split(';').ToList();

            var x = 0;
            foreach (var item in resultStr.TakeWhile(item => x < result.Count))
            {
                if (double.TryParse(item, out var temp))
                    result[x] = temp;
                x++;
            }

            return result;
        }

        public IFakturBuilder CalcTotal()
        {
            _aggRoot.Total = _aggRoot.ListItem.Sum(x => x.Total);
            _aggRoot.GrandTotal = _aggRoot.Total - _aggRoot.DiscountLain + _aggRoot.BiayaLain;
            _aggRoot.KurangBayar = _aggRoot.GrandTotal - _aggRoot.UangMuka;
            return this;
        }

        public IFakturBuilder ClearItem()
        {
            _aggRoot.ListItem.Clear();
            return this;
        }
    }
}