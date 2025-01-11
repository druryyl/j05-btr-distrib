using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.DriverAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.application.SupportContext.TglJamAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.DriverAgg;
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
        IFakturBuilder Driver(IDriverKey driverKey);
        IFakturBuilder TermOfPayment(TermOfPaymentEnum termOfPayment);
        IFakturBuilder DueDate(DateTime dueDate);

        IFakturBuilder AddItem(IBrgKey brgKey, string stokHrgStr, string qtyString, string hrgInputStr, string discountString, decimal dppProsen, decimal ppnProsen);
        IFakturBuilder ClearItem();

        IFakturBuilder FakturPajak(string noSeriFakturPajak);
        IFakturBuilder FpKeluaran(string fpKeluaranId);

        IFakturBuilder Void(IUserKey userKey);
        IFakturBuilder ReActivate(IUserKey userKey);
        IFakturBuilder User(IUserKey user);
        IFakturBuilder Cash(decimal cash);
        IFakturBuilder CalcTotal();
        IFakturBuilder Note(string note);
    }

    public class FakturBuilder : IFakturBuilder
    {
        private FakturModel _aggRoot = new FakturModel();
        private readonly IFakturDal _fakturDal;
        private readonly IFakturItemDal _fakturItemDal;
        private readonly IFakturDiscountDal _fakturDiscountDal;

        private readonly ICustomerDal _customerDal;
        private readonly ISalesPersonDal _salesPersonDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IDriverDal _driverDal;
        private readonly IBrgBuilder _brgBuilder;
        private readonly ITglJamDal _dateTime;
        private readonly ICreateFakturItemWorker _createFakturItemWorker;

        public FakturBuilder(IFakturDal fakturDal,
            IFakturItemDal fakturItemDal,
            IFakturDiscountDal fakturDiscountDal,
            ICustomerDal customerDal,
            ISalesPersonDal salesPersonDal,
            IWarehouseDal warehouseDal,
            IBrgBuilder brgBuilder,
            ITglJamDal dateTime,
            ICreateFakturItemWorker createFakturItemWorker,
            IDriverDal driverDal)
        {
            _fakturDal = fakturDal;
            _fakturItemDal = fakturItemDal;
            _fakturDiscountDal = fakturDiscountDal;

            _customerDal = customerDal;
            _salesPersonDal = salesPersonDal;
            _warehouseDal = warehouseDal;
            _brgBuilder = brgBuilder;
            _dateTime = dateTime;
            _createFakturItemWorker = createFakturItemWorker;
            _driverDal = driverDal;
        }

        public FakturModel Build()
        {
            _aggRoot.RemoveNull();
            return _aggRoot;
        }

        #region INITIATOR
        public IFakturBuilder CreateNew(IUserKey userKey)
        {
            _aggRoot = new FakturModel
            {
                CreateTime = _dateTime.Now,
                LastUpdate = new DateTime(3000, 1, 1),
                UserId = userKey.UserId,
                VoidDate = new DateTime(3000, 1, 1),
                UserIdVoid = userKey.UserId,
                ListItem = new List<FakturItemModel>(),


            };
            return this;
        }

        public IFakturBuilder Load(IFakturKey fakturKey)
        {
            _aggRoot = _fakturDal.GetData(fakturKey)
                       ?? throw new KeyNotFoundException($"Faktur not found ({fakturKey.FakturId})");
            _aggRoot.ListItem = _fakturItemDal.ListData(fakturKey)?.ToList()
                                ?? new List<FakturItemModel>();
            var allDiscount = _fakturDiscountDal.ListData(fakturKey)?.ToList()
                              ?? new List<FakturDiscountModel>();

            foreach (var item in _aggRoot.ListItem)
            {
                var brg = _brgBuilder.Load(item).Build();
                item.ListDiscount = allDiscount.Where(x => x.FakturItemId == item.FakturItemId).ToList();
            }

            return this;
        }

        public IFakturBuilder Attach(FakturModel faktur)
        {
            _aggRoot = faktur;
            return this;
        }
        #endregion

        #region HEADER
        public IFakturBuilder FakturDate(DateTime fakturDate)
        {
            TimeSpan timespan = DateTime.Now - DateTime.Now.Date;
            _aggRoot.FakturDate = fakturDate
                .AddHours(timespan.Hours)
                .AddMinutes(timespan.Minutes)
                .AddSeconds(timespan.Seconds);
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
            _aggRoot.HargaTypeId = customer.HargaTypeId;
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

        public IFakturBuilder Driver(IDriverKey driverKey)
        {
            var driver = _driverDal.GetData(driverKey);
            if (driver is null)
                return this;
            
            _aggRoot.DriverId = driver.DriverId;
            _aggRoot.DriverName = driver.DriverName;
            return this;
        }

        public IFakturBuilder TermOfPayment(TermOfPaymentEnum termOfPayment)
        {
            _aggRoot.TermOfPayment = termOfPayment;
            return this;
        }

        public IFakturBuilder DueDate(DateTime dueDate)
        {
            _aggRoot.DueDate = dueDate;
            return this;
        }
        public IFakturBuilder FakturPajak(string noSeriFakturPajak)
        {
            _aggRoot.NoFakturPajak = noSeriFakturPajak;
            return this;
        }
        public IFakturBuilder FpKeluaran(string fpKeluaranId)
        {
            _aggRoot.FpKeluaranId = fpKeluaranId;
            return this;
        }
        #endregion

        #region GRID
        public IFakturBuilder AddItem(IBrgKey brgKey, string stokHrgStr, string qtyInputStr, string hrgInputStr,
            string discInputStr, decimal dppProsen, decimal ppnProsen)
        {
            var item = _createFakturItemWorker.Execute(
                new CreateFakturItemRequest(brgKey.BrgId, qtyInputStr, discInputStr, hrgInputStr, 
                dppProsen, ppnProsen, _aggRoot.HargaTypeId, _aggRoot.WarehouseId));

            var noUrutMax = _aggRoot.ListItem
                .DefaultIfEmpty(new FakturItemModel() { NoUrut = 0 })
                .Max(x => x.NoUrut);
            item.NoUrut = noUrutMax + 1;

            _aggRoot.ListItem.Add(item);
            return this;
        }

        #endregion

        #region STATE
        public IFakturBuilder Void(IUserKey userKey)
        {
            _aggRoot.VoidDate = _dateTime.Now;
            _aggRoot.UserIdVoid = userKey.UserId;
            return this;
        }

        public IFakturBuilder ReActivate(IUserKey userKey)
        {
            _aggRoot.VoidDate = new DateTime(3000, 1, 1);
            _aggRoot.UserIdVoid = string.Empty;
            return this;
        }

        public IFakturBuilder User(IUserKey user)
        {
            _aggRoot.UserId = user.UserId;
            return this;
        }
        
        public IFakturBuilder ClearItem()
        {
            _aggRoot.ListItem.Clear();
            return this;
        }
        #endregion

        public IFakturBuilder CalcTotal()
        {
            _aggRoot.Total = _aggRoot.ListItem.Sum(x => x.SubTotal);
            _aggRoot.Discount = _aggRoot.ListItem.Sum(x => x.DiscRp);
            _aggRoot.Tax = _aggRoot.ListItem.Sum(x => x.PpnRp);
            _aggRoot.GrandTotal = _aggRoot.ListItem.Sum(x => x.Total);
            _aggRoot.KurangBayar = _aggRoot.GrandTotal - _aggRoot.UangMuka;
            return this;
        }

        public IFakturBuilder Cash(decimal cash)
        {
            _aggRoot.UangMuka = cash;
            CalcTotal();
            return this;
        }

        public IFakturBuilder Note(string note)
        {
            _aggRoot.Note = note;
            return this;
        }
    }
}


