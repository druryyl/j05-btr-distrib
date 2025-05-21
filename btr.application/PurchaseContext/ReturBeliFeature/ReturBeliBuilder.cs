using System;
using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.PurchaseContext.ReturBeliFeature;
using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.application.SupportContext.TglJamAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.PurchaseContext.ReturBeliFeature;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.PurchaseContext.ReturBeliAgg
{
    public interface IReturBeliBuilder : INunaBuilder<ReturBeliModel>
    {
        IReturBeliBuilder CreateNew(IUserKey userKey);
        IReturBeliBuilder Load(IReturBeliKey returBeliKey);
        IReturBeliBuilder Attach(ReturBeliModel returBeli);

        IReturBeliBuilder ReturBeliDate(DateTime returBeliDate);
        IReturBeliBuilder ReturBeliCode(string returBeliCode);

        IReturBeliBuilder Supplier(ISupplierKey supplierKey);
        IReturBeliBuilder Warehouse(IWarehouseKey warehouseKey);
        IReturBeliBuilder TermOfPayment(TermOfPaymentEnum termOfPayment);
        IReturBeliBuilder DueDate(DateTime dueDate);

        IReturBeliBuilder AddItem(IBrgKey brgKey, string hrgInputStr, string qtyString, string discountString, decimal dppProsen, decimal ppnProsen);
        IReturBeliBuilder ClearItem();

        IReturBeliBuilder FakturPajak(string noSeriFakturPajak);

        IReturBeliBuilder Void(IUserKey userKey);
        IReturBeliBuilder ReActivate(IUserKey userKey);
        IReturBeliBuilder User(IUserKey user);
        IReturBeliBuilder CalcTotal();
        IReturBeliBuilder IsPosted(bool isPosted);
    }

    public class ReturBeliBuilder : IReturBeliBuilder
    {
        private ReturBeliModel _aggRoot = new ReturBeliModel();
        private readonly IReturBeliDal _returBeliDal;
        private readonly IReturBeliItemDal _returBeliItemDal;
        private readonly IReturBeliDiscDal _returBeliDiscDal;

        private readonly ISupplierDal _supplierDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IBrgBuilder _brgBuilder;
        private readonly ITglJamDal _dateTime;
        private readonly ICreateReturBeliItemWorker _createReturBeliItemWorker;

        public ReturBeliBuilder(IReturBeliDal returBeliDal,
            IReturBeliItemDal returBeliItemDal,
            IReturBeliDiscDal returBeliDiscDal,
            ISupplierDal supplierDal,
            IWarehouseDal warehouseDal,
            IBrgBuilder brgBuilder,
            ITglJamDal dateTime,
            ICreateReturBeliItemWorker createReturBeliItemWorker)
        {
            _returBeliDal = returBeliDal;
            _returBeliItemDal = returBeliItemDal;
            _returBeliDiscDal = returBeliDiscDal;

            _supplierDal = supplierDal;
            _warehouseDal = warehouseDal;
            _brgBuilder = brgBuilder;
            _dateTime = dateTime;
            _createReturBeliItemWorker = createReturBeliItemWorker;
        }

        public ReturBeliModel Build()
        {
            _aggRoot.RemoveNull();
            return _aggRoot;
        }

        #region INITIATOR
        public IReturBeliBuilder CreateNew(IUserKey userKey)
        {
            _aggRoot = new ReturBeliModel
            {
                CreateTime = _dateTime.Now,
                LastUpdate = new DateTime(3000, 1, 1),
                UserId = userKey.UserId,
                VoidDate = new DateTime(3000, 1, 1),
                UserIdVoid = userKey.UserId,
                ListItem = new List<ReturBeliItemModel>(),
            };
            return this;
        }

        public IReturBeliBuilder Load(IReturBeliKey returBeliKey)
        {
            _aggRoot = _returBeliDal.GetData(returBeliKey)
                       ?? throw new KeyNotFoundException($"ReturBeli not found ({returBeliKey.ReturBeliId})");
            var listItem = _returBeliItemDal.ListData(returBeliKey)?.ToList()
                                ?? new List<ReturBeliItemModel>();
            _aggRoot.ListItem = listItem.OrderBy(x => x.NoUrut).ToList();
            var allDiscount = _returBeliDiscDal.ListData(returBeliKey)?.ToList()
                              ?? new List<ReturBeliDiscModel>();

            foreach (var item in _aggRoot.ListItem)
            {
                var brg = _brgBuilder.Load(item).Build();
                item.ListDisc = allDiscount.Where(x => x.ReturBeliItemId == item.ReturBeliItemId).ToList();
            }

            return this;
        }

        public IReturBeliBuilder Attach(ReturBeliModel returBeli)
        {
            _aggRoot = returBeli;
            return this;
        }
        #endregion

        #region HEADER
        public IReturBeliBuilder ReturBeliDate(DateTime returBeliDate)
        {
            TimeSpan timespan = DateTime.Now - DateTime.Now.Date;
            _aggRoot.ReturBeliDate = returBeliDate
                .AddHours(timespan.Hours)
                .AddMinutes(timespan.Minutes)
                .AddSeconds(timespan.Seconds);
            return this;
        }

        public IReturBeliBuilder ReturBeliCode(string code)
        {
            _aggRoot.ReturBeliCode = code;
            return this;
        }

        public IReturBeliBuilder Supplier(ISupplierKey supplierKey)
        {
            var supplier = _supplierDal.GetData(supplierKey)
                           ?? throw new KeyNotFoundException($"CustomerId not found ({supplierKey.SupplierId})");
            _aggRoot.SupplierId = supplier.SupplierId;
            _aggRoot.SupplierName = supplier.SupplierName;
            return this;
        }

        public IReturBeliBuilder Warehouse(IWarehouseKey warehouseKey)
        {
            var warehouse = _warehouseDal.GetData(warehouseKey)
                            ?? throw new KeyNotFoundException($"WarehouseId not found ({warehouseKey.WarehouseId})");
            _aggRoot.WarehouseId = warehouse.WarehouseId;
            _aggRoot.WarehouseName = warehouse.WarehouseName;
            return this;
        }

        public IReturBeliBuilder TermOfPayment(TermOfPaymentEnum termOfPayment)
        {
            _aggRoot.TermOfPayment = termOfPayment;
            return this;
        }

        public IReturBeliBuilder DueDate(DateTime dueDate)
        {
            _aggRoot.DueDate = dueDate;
            return this;
        }
        public IReturBeliBuilder FakturPajak(string noSeriFakturPajak)
        {
            _aggRoot.NoFakturPajak = noSeriFakturPajak;
            return this;
        }
        #endregion

        #region GRID
        public IReturBeliBuilder AddItem(IBrgKey brgKey, string hrgInputStr, string qtyInputStr,
            string discInputStr, decimal dppProsen, decimal ppnProsen)
        {
            var item = _createReturBeliItemWorker.Execute(
                new CreateReturBeliItemRequest(
                    brgKey.BrgId, hrgInputStr, qtyInputStr,
                    discInputStr, dppProsen, ppnProsen, false));

            var noUrutMax = _aggRoot.ListItem
                .DefaultIfEmpty(new ReturBeliItemModel() { NoUrut = 0 })
                .Max(x => x.NoUrut);
            item.NoUrut = noUrutMax + 1;

            _aggRoot.ListItem.Add(item);
            return this;
        }

        #endregion

        #region STATE
        public IReturBeliBuilder Void(IUserKey userKey)
        {
            _aggRoot.VoidDate = _dateTime.Now;
            _aggRoot.UserIdVoid = userKey.UserId;
            return this;
        }

        public IReturBeliBuilder ReActivate(IUserKey userKey)
        {
            _aggRoot.VoidDate = new DateTime(3000, 1, 1);
            _aggRoot.UserIdVoid = string.Empty;
            return this;
        }

        public IReturBeliBuilder User(IUserKey user)
        {
            _aggRoot.UserId = user.UserId;
            return this;
        }

        public IReturBeliBuilder ClearItem()
        {
            _aggRoot.ListItem.Clear();
            return this;
        }
        #endregion

        public IReturBeliBuilder CalcTotal()
        {
            _aggRoot.Total = _aggRoot.ListItem.Sum(x => x.SubTotal);
            _aggRoot.Disc = _aggRoot.ListItem.Sum(x => x.DiscRp);
            _aggRoot.Tax = _aggRoot.ListItem.Sum(x => x.PpnRp);
            _aggRoot.GrandTotal = _aggRoot.ListItem.Sum(x => x.Total);
            return this;
        }

        public IReturBeliBuilder IsPosted(bool isPosted)
        {
            _aggRoot.IsStokPosted = isPosted;
            return this;
        }
    }
}


