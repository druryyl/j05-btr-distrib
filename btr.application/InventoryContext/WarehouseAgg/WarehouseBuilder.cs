using System.Collections.Generic;
using btr.application.InventoryContext.WarehouseAgg.Contracts;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.InventoryContext.WarehouseAgg.Workers
{
    public interface IWarehouseBuilder : INunaBuilder<WarehouseModel>
    {
        IWarehouseBuilder Create();
        IWarehouseBuilder Load(IWarehouseKey warehouseKey);
        IWarehouseBuilder Attach(WarehouseModel model);
        IWarehouseBuilder Name(string name);
    }

    public class WarehouseBuilder : IWarehouseBuilder
    {
        private WarehouseModel _aggRoot = new WarehouseModel();
        private readonly IWarehouseDal _warehouseDal;

        public WarehouseBuilder(IWarehouseDal warehouseDal)
        {
            _warehouseDal = warehouseDal;
        }

        public WarehouseModel Build()
        {
            _aggRoot.RemoveNull();
            return _aggRoot;
        }

        public IWarehouseBuilder Create()
        {
            _aggRoot = new WarehouseModel
            {
                WarehouseName = string.Empty,
                WarehouseId = string.Empty
            };
            return this;
        }

        public IWarehouseBuilder Load(IWarehouseKey warehouseKey)
        {
            _aggRoot = _warehouseDal.GetData(warehouseKey)
                       ?? throw new KeyNotFoundException($"WarehouseId not found ({warehouseKey.WarehouseId})");
            return this;
        }

        public IWarehouseBuilder Name(string name)
        {
            _aggRoot.WarehouseName = name;
            return this;
        }

        public IWarehouseBuilder Attach(WarehouseModel model)
        {
            _aggRoot = model;
            return this;
        }
    }
}