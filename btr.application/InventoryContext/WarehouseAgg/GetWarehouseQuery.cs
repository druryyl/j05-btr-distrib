using System.Threading;
using System.Threading.Tasks;
using btr.application.InventoryContext.WarehouseAgg.Workers;
using btr.domain.InventoryContext.WarehouseAgg;
using Dawn;
using Mapster;
using MediatR;

namespace btr.application.InventoryContext.WarehouseAgg.UseCases
{
    public class GetWarehouseQuery : IRequest<WarehouseModel>, IWarehouseKey
    {
        public GetWarehouseQuery(string id) => WarehouseId = id;
        public string WarehouseId { get; }
    }

    public class GetWarehouseHandler : IRequestHandler<GetWarehouseQuery, WarehouseModel>
    {
        private WarehouseModel _aggRoot = new WarehouseModel();
        private readonly IWarehouseBuilder _builder;

        public GetWarehouseHandler(IWarehouseBuilder builder)
        {
            _builder = builder;
        }

        public Task<WarehouseModel> Handle(GetWarehouseQuery request, CancellationToken cancellationToken)
        {
            //  GUARD
            Guard.Argument(() => request).NotNull()
                .Member(x => x.WarehouseId, y => y.NotEmpty());

            //  BUILD
            _aggRoot = _builder
                .Load(request)
                .Build();

            //  APPLY
            return Task.FromResult(_aggRoot);
        }
    }
}