using btr.domain.InventoryContext.WarehouseAgg;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using btr.domain.SupportContext.DocAgg;
using btr.application.SalesContext.FakturAgg.Workers;

namespace btr.application.SupportContext.DocAgg
{
    public class CreateDocOnSavedFakturEventHandler : INotificationHandler<SavedFakturEvent>
    {
        private DocModel _aggregate;
        private readonly IDocBuilder _builder;
        private readonly IDocWriter _writer;

        public CreateDocOnSavedFakturEventHandler(IDocBuilder builder, 
            IDocWriter writer)
        {
            _builder = builder;
            _writer = writer;
        }

        public Task Handle(SavedFakturEvent notification, CancellationToken cancellationToken)
        {
            //  BUILD
            _aggregate = _builder
                .LoadOrCreate(new DocModel(notification.Aggregate.FakturId))
                .Code(notification.Aggregate.FakturCode)
                .Warehouse(new WarehouseModel(notification.Aggregate.WarehouseId))
                .Description($"{notification.Aggregate.CustomerName}")
                .Queue()
                .Build();

            //  WRITER
            _writer.Save(ref _aggregate);

            //  RETURN
            return Task.CompletedTask;
        }
    }
}
