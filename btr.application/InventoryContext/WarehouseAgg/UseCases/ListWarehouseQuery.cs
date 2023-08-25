using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using btr.application.InventoryContext.WarehouseAgg.Contracts;
using btr.domain.InventoryContext.WarehouseAgg;
using Mapster;
using MediatR;

namespace btr.application.InventoryContext.WarehouseAgg.UseCases
{
    public class ListWarehouseQuery : IRequest<IEnumerable<WarehouseModel>>
    {
    }

    public class ListWarehouseHandler : IRequestHandler<ListWarehouseQuery, IEnumerable<WarehouseModel>>
    {
        private readonly IWarehouseDal _warehouseDal;

        public ListWarehouseHandler(IWarehouseDal warehouseDal)
        {
            _warehouseDal = warehouseDal;
        }

        public Task<IEnumerable<WarehouseModel>> Handle(ListWarehouseQuery request,
            CancellationToken cancellationToken)
        {
            //  BUILD
            var result = _warehouseDal.ListData()
                         ?? throw new KeyNotFoundException("Warehouse not found");

            //  APPLY
            return Task.FromResult(GenResponse(result));
        }

        private IEnumerable<WarehouseModel> GenResponse(IEnumerable<WarehouseModel> listWarehouse)
        {
            var result = listWarehouse.Adapt<IEnumerable<WarehouseModel>>();
            return result;
        }
    }
}