using System;
using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.application.SupportContext.TglJamAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.PurchaseContext.InvoiceAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.PurchaseContext.InvoiceAgg
{
    public interface IInvoiceBuilder : INunaBuilder<InvoiceModel>
    {
        IInvoiceBuilder CreateNew(IUserKey userKey);
        IInvoiceBuilder Load(IInvoiceKey invoiceKey);
        IInvoiceBuilder Attach(InvoiceModel invoice);

        IInvoiceBuilder InvoiceDate(DateTime invoiceDate);
        IInvoiceBuilder InvoiceCode(string invoiceCode);

        IInvoiceBuilder Supplier(ISupplierKey supplierKey);
        IInvoiceBuilder Warehouse(IWarehouseKey warehouseKey);
        IInvoiceBuilder TermOfPayment(TermOfPaymentEnum termOfPayment);
        IInvoiceBuilder DueDate(DateTime dueDate);

        IInvoiceBuilder AddItem(IBrgKey brgKey, string hrgInputStr, string qtyString, string discountString, decimal dppProsen, decimal ppnProsen);
        IInvoiceBuilder ClearItem();

        IInvoiceBuilder FakturPajak(string noSeriFakturPajak);

        IInvoiceBuilder Void(IUserKey userKey);
        IInvoiceBuilder ReActivate(IUserKey userKey);
        IInvoiceBuilder User(IUserKey user);
        IInvoiceBuilder CalcTotal();
        IInvoiceBuilder IsPosted(bool isPosted);
    }

    public class InvoiceBuilder : IInvoiceBuilder
    {
        private InvoiceModel _aggRoot = new InvoiceModel();
        private readonly IInvoiceDal _invoiceDal;
        private readonly IInvoiceItemDal _invoiceItemDal;
        private readonly IInvoiceDiscDal _invoiceDiscDal;

        private readonly ISupplierDal _supplierDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IBrgBuilder _brgBuilder;
        private readonly ITglJamDal _dateTime;
        private readonly ICreateInvoiceItemWorker _createInvoiceItemWorker;

        public InvoiceBuilder(IInvoiceDal invoiceDal,
            IInvoiceItemDal invoiceItemDal,
            IInvoiceDiscDal invoiceDiscDal,
            ISupplierDal supplierDal,
            IWarehouseDal warehouseDal,
            IBrgBuilder brgBuilder,
            ITglJamDal dateTime,
            ICreateInvoiceItemWorker createInvoiceItemWorker)
        {
            _invoiceDal = invoiceDal;
            _invoiceItemDal = invoiceItemDal;
            _invoiceDiscDal = invoiceDiscDal;

            _supplierDal = supplierDal;
            _warehouseDal = warehouseDal;
            _brgBuilder = brgBuilder;
            _dateTime = dateTime;
            _createInvoiceItemWorker = createInvoiceItemWorker;
        }

        public InvoiceModel Build()
        {
            _aggRoot.RemoveNull();
            return _aggRoot;
        }

        #region INITIATOR
        public IInvoiceBuilder CreateNew(IUserKey userKey)
        {
            _aggRoot = new InvoiceModel
            {
                CreateTime = _dateTime.Now,
                LastUpdate = new DateTime(3000, 1, 1),
                UserId = userKey.UserId,
                VoidDate = new DateTime(3000, 1, 1),
                UserIdVoid = userKey.UserId,
                ListItem = new List<InvoiceItemModel>(),
            };
            return this;
        }

        public IInvoiceBuilder Load(IInvoiceKey invoiceKey)
        {
            _aggRoot = _invoiceDal.GetData(invoiceKey)
                       ?? throw new KeyNotFoundException($"Invoice not found ({invoiceKey.InvoiceId})");
            var listItem = _invoiceItemDal.ListData(invoiceKey)?.ToList()
                                ?? new List<InvoiceItemModel>();
            _aggRoot.ListItem = listItem.OrderBy(x => x.NoUrut).ToList();
            var allDiscount = _invoiceDiscDal.ListData(invoiceKey)?.ToList()
                              ?? new List<InvoiceDiscModel>();

            foreach (var item in _aggRoot.ListItem)
            {
                var brg = _brgBuilder.Load(item).Build();
                item.ListDisc = allDiscount.Where(x => x.InvoiceItemId == item.InvoiceItemId).ToList();
            }

            return this;
        }

        public IInvoiceBuilder Attach(InvoiceModel invoice)
        {
            _aggRoot = invoice;
            return this;
        }
        #endregion

        #region HEADER
        public IInvoiceBuilder InvoiceDate(DateTime invoiceDate)
        {
            TimeSpan timespan = DateTime.Now - DateTime.Now.Date;
            _aggRoot.InvoiceDate = invoiceDate
                .AddHours(timespan.Hours)
                .AddMinutes(timespan.Minutes)
                .AddSeconds(timespan.Seconds);
            return this;
        }

        public IInvoiceBuilder InvoiceCode(string code)
        {
            _aggRoot.InvoiceCode = code;
            return this;
        }

        public IInvoiceBuilder Supplier(ISupplierKey supplierKey)
        {
            var supplier = _supplierDal.GetData(supplierKey)
                           ?? throw new KeyNotFoundException($"CustomerId not found ({supplierKey.SupplierId})");
            _aggRoot.SupplierId = supplier.SupplierId;
            _aggRoot.SupplierName = supplier.SupplierName;
            return this;
        }

        public IInvoiceBuilder Warehouse(IWarehouseKey warehouseKey)
        {
            var warehouse = _warehouseDal.GetData(warehouseKey)
                            ?? throw new KeyNotFoundException($"WarehouseId not found ({warehouseKey.WarehouseId})");
            _aggRoot.WarehouseId = warehouse.WarehouseId;
            _aggRoot.WarehouseName = warehouse.WarehouseName;
            return this;
        }

        public IInvoiceBuilder TermOfPayment(TermOfPaymentEnum termOfPayment)
        {
            _aggRoot.TermOfPayment = termOfPayment;
            return this;
        }

        public IInvoiceBuilder DueDate(DateTime dueDate)
        {
            _aggRoot.DueDate = dueDate;
            return this;
        }
        public IInvoiceBuilder FakturPajak(string noSeriFakturPajak)
        {
            _aggRoot.NoFakturPajak = noSeriFakturPajak;
            return this;
        }
        #endregion

        #region GRID
        public IInvoiceBuilder AddItem(IBrgKey brgKey, string hrgInputStr, string qtyInputStr,
            string discInputStr, decimal dppProsen, decimal ppnProsen)
        {
            var item = _createInvoiceItemWorker.Execute(
                new CreateInvoiceItemRequest(
                    brgKey.BrgId, hrgInputStr, qtyInputStr, 
                    discInputStr, dppProsen, ppnProsen, true));

            var noUrutMax = _aggRoot.ListItem
                .DefaultIfEmpty(new InvoiceItemModel() { NoUrut = 0 })
                .Max(x => x.NoUrut);
            item.NoUrut = noUrutMax + 1;

            _aggRoot.ListItem.Add(item);
            return this;
        }

        #endregion

        #region STATE
        public IInvoiceBuilder Void(IUserKey userKey)
        {
            _aggRoot.VoidDate = _dateTime.Now;
            _aggRoot.UserIdVoid = userKey.UserId;
            return this;
        }

        public IInvoiceBuilder ReActivate(IUserKey userKey)
        {
            _aggRoot.VoidDate = new DateTime(3000, 1, 1);
            _aggRoot.UserIdVoid = string.Empty;
            return this;
        }

        public IInvoiceBuilder User(IUserKey user)
        {
            _aggRoot.UserId = user.UserId;
            return this;
        }

        public IInvoiceBuilder ClearItem()
        {
            _aggRoot.ListItem.Clear();
            return this;
        }
        #endregion

        public IInvoiceBuilder CalcTotal()
        {
            _aggRoot.Total = _aggRoot.ListItem.Sum(x => x.SubTotal);
            _aggRoot.Disc = _aggRoot.ListItem.Sum(x => x.DiscRp);
            _aggRoot.Tax = _aggRoot.ListItem.Sum(x => x.PpnRp);
            _aggRoot.GrandTotal = _aggRoot.ListItem.Sum(x => x.Total);
            return this;
        }

        public IInvoiceBuilder IsPosted(bool isPosted)
        {
            _aggRoot.IsStokPosted = isPosted;
            return this;
        }
    }
}


