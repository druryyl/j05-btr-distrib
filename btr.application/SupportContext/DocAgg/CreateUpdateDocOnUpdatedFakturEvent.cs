using btr.application.SalesContext.FakturAgg.UseCases;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SupportContext.PrintManagerAgg;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace btr.application.SupportContext.DocAgg
{
    public class CreateUpdateDocOnCreatedFakturEventHandler : INotificationHandler<UpdatedFakturEvent>
    {
        private DocModel _aggregate;
        private readonly IDocBuilder _builder;
        private readonly IDocWriter _writer;

        public CreateUpdateDocOnCreatedFakturEventHandler(IDocBuilder builder,
            IDocWriter writer)
        {
            _builder = builder;
            _writer = writer;
        }

        public Task Handle(UpdatedFakturEvent notification, CancellationToken cancellationToken)
        {
            //  BUILD
            _aggregate = _builder
                .LoadOrCreate(new DocModel(notification.Aggregate.FakturId))
                .Warehouse(new WarehouseModel(notification.Aggregate.WarehouseId))
                .Queue()
                .Build();

            //  WRITER
            _writer.Save(ref _aggregate);

            //  RETURN
            return Task.CompletedTask;

        }
    }
}
