using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using Dawn;
using Mapster;
using MediatR;

namespace btr.application.SalesContext.FakturAgg.UseCases
{
    public class UpdateFakturCommand : IRequest<UpdateFakturResponse>, ICustomerKey,
        ISalesPersonKey, IWarehouseKey, IUserKey, IFakturKey
    {
        public string FakturId { get; set; }
        public string FakturDate { get; set; }
        public string CustomerId { get; set; }
        public string SalesPersonId { get; set; }
        public string WarehouseId { get; set; }
        public string RencanaKirimDate { get; set; }
        public int TermOfPayment { get; set; }
        public string UserId { get; set; }
        public IEnumerable<UpdateFakturCommandItem> ListBrg { get; set; }
    }

    public class UpdateFakturCommandItem : IBrgKey
    {
        public string BrgId { get; set; }
        public string QtyString { get; set; }
        public string DiscountString { get; set; }
        public decimal PpnProsen { get; set; }
    }

    public class UpdateFakturResponse
    {
        public string FakturId { get; set; }
        public string FakturDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
    }

    public class UpdateFakturHandler : IRequestHandler<UpdateFakturCommand, UpdateFakturResponse>
    {
        private FakturModel _aggRoot = new FakturModel();
        private readonly IFakturBuilder _builder;
        private readonly IFakturWriter _writer;
        private readonly IMediator _mediator;

        public UpdateFakturHandler(IFakturBuilder builder, 
            IFakturWriter writer, 
            IMediator mediator)
        {
            _builder = builder;
            _writer = writer;
            _mediator = mediator;
        }

        public Task<UpdateFakturResponse> Handle(UpdateFakturCommand request, CancellationToken cancellationToken)
        {
            //  GUARD
            Guard.Argument(() => request).NotNull()
                .Member(x => x.FakturDate, y => y.ValidDate("yyyy-MM-dd"))
                .Member(x => x.CustomerId, y => y.NotEmpty())
                .Member(x => x.SalesPersonId, y => y.NotEmpty())
                .Member(x => x.WarehouseId, y => y.NotEmpty());

            //  BUILD
            _aggRoot = _builder
                .Load(request)
                .FakturDate(request.FakturDate.ToDate(DateFormatEnum.YMD))
                .Customer(request)
                .SalesPerson(request)
                .Warehouse(request)
                .TglRencanaKirim(request.RencanaKirimDate.ToDate(DateFormatEnum.YMD))
                .ClearItem()
                .Build();
            foreach (var item in request.ListBrg)
            {
                _aggRoot = _builder
                    .Attach(_aggRoot)
                    .AddItem(item, item.QtyString, item.DiscountString, item.PpnProsen)
                    .Build();
            }
            _aggRoot = _builder
                .Attach(_aggRoot)
                .CalcTotal()
                .Build();

            //  APPLY
            _writer.Save(ref _aggRoot);
            _mediator.Publish(new UpdatedFakturEvent(request, _aggRoot), cancellationToken);
            return Task.FromResult(GenResponse());
        }

        private UpdateFakturResponse GenResponse()
        {
            var result = _aggRoot.Adapt<UpdateFakturResponse>();
            return result;
        }
    }

    public class UpdatedFakturEvent : INotification
    {
        public UpdatedFakturEvent(UpdateFakturCommand command, FakturModel aggregate)
        {
            Command = command;
            Aggregate = aggregate;
        }
        public UpdateFakturCommand Command { get; set; }
        public FakturModel Aggregate { get; set; }
    }
}