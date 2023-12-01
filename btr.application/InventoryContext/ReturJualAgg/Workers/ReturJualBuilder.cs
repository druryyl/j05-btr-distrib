using btr.application.InventoryContext.ReturJualAgg.Contracts;
using btr.domain.InventoryContext.ReturJualAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using btr.application.InventoryContext.DriverAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.domain.InventoryContext.DriverAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.domain.SupportContext.UserAgg;
using Mapster;

namespace btr.application.InventoryContext.ReturJualAgg.Workers
{
    public interface IReturJualBuilder : INunaBuilder<ReturJualModel>
    {
        IReturJualBuilder Load(IReturJualKey returJualKey);
        IReturJualBuilder Create();
        IReturJualBuilder Attach(ReturJualModel model);
        IReturJualBuilder Customer(ICustomerKey customerKey);
        IReturJualBuilder Warehouse(IWarehouseKey warehouseKey);
        IReturJualBuilder SalesPerson(ISalesPersonKey salesPersonKey);
        IReturJualBuilder Driver(IDriverKey driverKey);
        IReturJualBuilder ReturJualDate(DateTime returJualDate);
        IReturJualBuilder AddItem(ReturJualItemModel item);
        IReturJualBuilder User(string userId);
        
    }

    public class ReturJualBuilder : IReturJualBuilder
    {
        private ReturJualModel _aggregate;
        private readonly IReturJualDal _returJualDal;
        private readonly IReturJualItemDal _returJualItemDal;
        private readonly IReturJualItemQtyHrgDal _returJualItemQtyHrgDal;
        private readonly IReturJualItemDiscDal _returJualItemDiscDal;

        private readonly ICustomerDal _customerDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly ISalesPersonDal _salesPersonDal;
        private readonly IDriverDal _driverDal;
        
        public ReturJualBuilder(IReturJualDal returJualDal, 
            IReturJualItemDal returJualItemDal, 
            IReturJualItemQtyHrgDal returJualItemQtyHrgDal, 
            IReturJualItemDiscDal returJualItemDiscDal, ICustomerDal customerDal, IWarehouseDal warehouseDal, ISalesPersonDal salesPersonDal, IDriverDal driverDal)
        {
            _returJualDal = returJualDal;
            _returJualItemDal = returJualItemDal;
            _returJualItemQtyHrgDal = returJualItemQtyHrgDal;
            _returJualItemDiscDal = returJualItemDiscDal;
            _customerDal = customerDal;
            _warehouseDal = warehouseDal;
            _salesPersonDal = salesPersonDal;
            _driverDal = driverDal;
        }

        public IReturJualBuilder Load(IReturJualKey returJualKey)
        {
            _aggregate = _returJualDal.GetData(returJualKey)
                ?? throw new Exception("Retur Jual tidak ditemukan");
            
            _aggregate.ListItem = _returJualItemDal.ListData(returJualKey)?.ToList()
                                  ?? new List<ReturJualItemModel>();
            //var listQtyHrg = _returJualItemQtyHrgDal.ListData(returJualKey)?.ToList()
            //    ?? new List<ReturJualItemQtyHrgModel>();
            //var listDisc = _returJualItemDiscDal.ListData(returJualKey)?.ToList()
            //    ?? new List<ReturJualItemDiscModel>();

            //_aggregate.ListItem.ForEach(item =>
            //{
            //    item.ListQtyHrg = listQtyHrg.Where(x => x.ReturJualItemId == item.ReturJualItemId).ToList();
            //    item.ListDisc = listDisc.Where(x => x.ReturJualItemId == item.ReturJualItemId).ToList();
            //});
            return this;
        }

        public IReturJualBuilder Create()
        {
            _aggregate = new ReturJualModel
            {
                ReturJualDate = new DateTime(3000,1,1),
                VoidDate = new DateTime(3000,1,1),
                ListItem = new List<ReturJualItemModel>()
            };
            return this;
        }

        public IReturJualBuilder Attach(ReturJualModel model)
        {
            _aggregate = model;
            return this;
        }

        public IReturJualBuilder Customer(ICustomerKey customerKey)
        {
            var customer = _customerDal.GetData(customerKey)
                ?? throw new KeyNotFoundException("Customer tidak ditemukan");
            _aggregate.CustomerId = customer.CustomerId;
            _aggregate.CustomerName = customer.CustomerCode;
            return this;
        }

        public IReturJualBuilder Warehouse(IWarehouseKey warehouseKey)
        {
            var warehouse = _warehouseDal.GetData(warehouseKey)
                ?? throw new KeyNotFoundException("Warehouse tidak ditemukan");
            _aggregate.WarehouseId = warehouse.WarehouseId;
            _aggregate.WarehouseName = warehouse.WarehouseName;
            return this;
        }

        public IReturJualBuilder SalesPerson(ISalesPersonKey salesPersonKey)
        {
            var salesPerson = _salesPersonDal.GetData(salesPersonKey)
                ?? throw new KeyNotFoundException("Sales Person tidak ditemukan");
            _aggregate.SalesPersonId = salesPerson.SalesPersonId;
            _aggregate.SalesPersonName = salesPerson.SalesPersonName;
            return this;
            
        }

        public IReturJualBuilder Driver(IDriverKey driverKey)
        {
            var driver = _driverDal.GetData(driverKey)
                ?? throw new KeyNotFoundException("Driver tidak ditemukan");
            _aggregate.DriverId = driver.DriverId;
            _aggregate.DriverName = driver.DriverName;
            return this;
        }

        public IReturJualBuilder ReturJualDate(DateTime returJualDate)
        {
            _aggregate.ReturJualDate = returJualDate;
            return this;
        }

        public IReturJualBuilder AddItem(ReturJualItemModel item)
        {
            var noUrut = _aggregate.ListItem
                .DefaultIfEmpty(new ReturJualItemModel { NoUrut = 0 })
                .Max(x => x.NoUrut);
            noUrut++;
            item.NoUrut = noUrut;

            _aggregate.ListItem.Add(item);
            _aggregate.Total = _aggregate.ListItem.Sum(x => x.SubTotal);
            _aggregate.DiscRp = _aggregate.ListItem.Sum(x => x.DiscRp);
            _aggregate.PpnRp = _aggregate.ListItem.Sum(x => x.PpnRp);
            _aggregate.GrandTotal = _aggregate.ListItem.Sum(x => x.Total);
            return this;
        }

        public IReturJualBuilder User(string userId)
        {
            _aggregate.UserId = userId;
            return this;    
        }

        public ReturJualModel Build()
        {
            _aggregate.RemoveNull();
            return _aggregate;
        }
    }
}
