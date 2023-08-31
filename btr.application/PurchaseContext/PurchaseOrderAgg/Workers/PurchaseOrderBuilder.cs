using System;
using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.PurchaseContext.PurchaseOrderAgg.Contracts;
using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.PurchaseContext.PurchaseOrderAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using FluentValidation;

namespace btr.application.PurchaseContext.PurchaseOrderAgg.Workers
{
    public interface IPurchaseOrderBuilder : INunaBuilder<PurchaseOrderModel>
    {
        //  creator
        IPurchaseOrderBuilder Create();
        IPurchaseOrderBuilder Load(IPurchaseOrderKey purchaseOrderKey);
        IPurchaseOrderBuilder Attach(PurchaseOrderModel purchaseOrder);
        
        //  builder
        IPurchaseOrderBuilder Supplier(ISupplierKey supplierKey);
        IPurchaseOrderBuilder Warehouse(IWarehouseKey warehouseKey);
        IPurchaseOrderBuilder DiscountLain(decimal  discount);
        IPurchaseOrderBuilder BiayaLain(decimal biayaLain);

        IPurchaseOrderBuilder AddItem(IBrgKey brgKey, int qty, string satuan, 
            decimal harga, decimal diskon, decimal tax);

        IPurchaseOrderBuilder RemoveItem(IBrgKey brgKey);

    }
    public class PurchaseOrderBuilder : IPurchaseOrderBuilder
    {
        private PurchaseOrderModel _aggRoot = new PurchaseOrderModel();
        private readonly IPurchaseOrderDal _purchaseOrderDal;
        private readonly IPurchaseOrderItemDal _purchaseOrderItemDal;
        private readonly ISupplierDal _supplierDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IBrgBuilder _brgBuilder;
        private readonly DateTimeProvider _dateTimeProvider;

        public PurchaseOrderBuilder(IPurchaseOrderDal purchaseOrderDal, 
            IPurchaseOrderItemDal purchaseOrderItemDal, 
            ISupplierDal supplierDal, 
            IWarehouseDal warehouseDal, 
            IBrgBuilder brgBuilder, 
            DateTimeProvider dateTimeProvider)
        {
            _purchaseOrderDal = purchaseOrderDal;
            _purchaseOrderItemDal = purchaseOrderItemDal;
            _supplierDal = supplierDal;
            _warehouseDal = warehouseDal;
            _brgBuilder = brgBuilder;
            _dateTimeProvider = dateTimeProvider;
        }

        public PurchaseOrderModel Build()
        {
            _aggRoot.RemoveNull();
            return _aggRoot;
        }

        public IPurchaseOrderBuilder Create()
        {
            _aggRoot = new PurchaseOrderModel
            {
                PurchaseOrderDate = _dateTimeProvider.Now,
                ListItem = new List<PurchaseOrderItemModel>()
            };
            return this;
        }

        public IPurchaseOrderBuilder Load(IPurchaseOrderKey purchaseOrderKey)
        {
            _aggRoot = _purchaseOrderDal.GetData(purchaseOrderKey)
                       ?? throw new KeyNotFoundException("Purchase Order ID invalid");
            _aggRoot.ListItem = _purchaseOrderItemDal.ListData(purchaseOrderKey)?.ToList()
                                ?? new List<PurchaseOrderItemModel>();
            return this;
        }

        public IPurchaseOrderBuilder Attach(PurchaseOrderModel purchaseOrder)
        {
            _aggRoot = purchaseOrder;
            return this;
        }

        public IPurchaseOrderBuilder Supplier(ISupplierKey supplierKey)
        {
            var supplier = _supplierDal.GetData(supplierKey)
                           ?? throw new KeyNotFoundException("Supplier ID invalid");
            _aggRoot.SupplierId = supplier.SupplierId;
            _aggRoot.SupplierName = supplier.SupplierName;
            return this;
        }

        public IPurchaseOrderBuilder Warehouse(IWarehouseKey warehouseKey)
        {
            var warehouse = _warehouseDal.GetData(warehouseKey)
                           ?? throw new KeyNotFoundException("Warehouse ID invalid");
            _aggRoot.WarehouseId = warehouse.WarehouseId;
            _aggRoot.WarehouseName = warehouse.WarehouseName;
            return this;
        }

        public IPurchaseOrderBuilder AddItem(IBrgKey brgKey, int qty, string satuan, decimal harga, decimal diskon, decimal tax)
        {
            //  brgid harus unik
            if (_aggRoot.ListItem.FirstOrDefault(x => x.BrgId == brgKey.BrgId) != null)
                throw new ArgumentException($"BrgID duplicated {brgKey.BrgId}");
            
            //  brgid harus valid
            var brg = _brgBuilder.Load(brgKey).Build();
            if (brg.ListSatuan.FirstOrDefault(x => x.Satuan == satuan) is null)
                throw new ArgumentException($"Satuan barang {brgKey.BrgId} invalid");

            //  validasi qty, harga, diskon dan tax
            if (qty <= 0) throw new ArgumentException("Qty invalid");
            if (harga < 0) throw new ArgumentException("Harga invalid");
            if (diskon < 0) throw new ArgumentException("Diskon invalid");
            if (tax < 0) throw new ArgumentException("Tax invalid");

            var no = _aggRoot.ListItem
                .DefaultIfEmpty(new PurchaseOrderItemModel { NoUrut = 0 })
                .Max(x => x.NoUrut);
            no++;
            var subTotal = qty * harga;
            var diskonRp = subTotal * diskon / 100;
            var taxRp = subTotal * tax / 100;
            var newItem = new PurchaseOrderItemModel
            {
                NoUrut = no,
                BrgId = brgKey.BrgId,
                BrgName = brg.BrgName,
                Qty = qty,
                Satuan = satuan,
                Harga = harga,
                SubTotal = subTotal,
                DiskonProsen = diskon,
                DiskonRp = diskonRp,
                TaxProsen = tax,
                TaxRp = taxRp,
                Total = subTotal - diskonRp + taxRp
            };
            _aggRoot.ListItem.Add(newItem);
            return this;
        }

        public IPurchaseOrderBuilder RemoveItem(IBrgKey brgKey)
        {
            _aggRoot.ListItem.RemoveAll(x => x.BrgId == brgKey.BrgId);
            return this;
        }

        public IPurchaseOrderBuilder DiscountLain(decimal nilai)
        {
            _aggRoot.DiscountLain = nilai;
            return this;
        }

        public IPurchaseOrderBuilder BiayaLain(decimal nilai)
        {
            _aggRoot.BiayaLain= nilai;
            return this;
        }
    }
}