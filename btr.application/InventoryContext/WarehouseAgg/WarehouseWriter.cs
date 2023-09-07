using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.InventoryContext.WarehouseAgg
{
    public interface IWarehouseWriter : INunaWriter<WarehouseModel>
    {
    }

    public class WarehouseWriter : IWarehouseWriter
    {
        private readonly IWarehouseDal _warehouseDal;
        private readonly INunaCounterBL _counter;

        public WarehouseWriter(IWarehouseDal warehouseDal, 
            INunaCounterBL counter)
        {
            _warehouseDal = warehouseDal;
            _counter = counter;
        }

        public void Save(ref WarehouseModel model)
        {
            if (model.WarehouseId.IsNullOrEmpty())
                model.WarehouseId = _counter.Generate("G", IDFormatEnum.Pnn);
            var db = _warehouseDal.GetData(model);
            if (db is null)
                _warehouseDal.Insert(model);
            else
                _warehouseDal.Update(model);
        }
    }
}
