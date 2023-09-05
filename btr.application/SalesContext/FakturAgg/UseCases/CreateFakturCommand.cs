using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using btr.application.InventoryContext.StokAgg;
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
    public class CreateFakturCommand : IRequest<CreateFakturResponse>, ICustomerKey,
        ISalesPersonKey, IWarehouseKey, IUserKey
    {
        public string FakturDate { get; set; }
        public string CustomerId { get; set; }
        public string SalesPersonId { get; set; }
        public string WarehouseId { get; set; }
        public string RencanaKirimDate { get; set; }
        public int TermOfPayment { get; set; }
        public string DueDate { get; set; }
        public string UserId { get; set; }
        public IEnumerable<CreateFakturCommandItem> ListBrg { get; set; }
    }

    public class CreateFakturCommandItem : IBrgKey
    {
        public string BrgId { get; set; }
        public string QtyString { get; set; }
        public string DiscountString { get; set; }
        public decimal PpnProsen { get; set; }
    }

    public class CreateFakturResponse
    {
        public string FakturId { get; set; }
        public string FakturDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
    }

    public class CreateFakturHandler : IRequestHandler<CreateFakturCommand, CreateFakturResponse>
    {
        private FakturModel _aggRoot = new FakturModel();
        private readonly IFakturBuilder _builder;
        private readonly IFakturWriter _writer;
        private readonly IMediator _mediator;

        public CreateFakturHandler(IFakturBuilder builder, 
            IFakturWriter writer, 
            IMediator mediator)
        {
            _builder = builder;
            _writer = writer;
            _mediator = mediator;
        }

        public Task<CreateFakturResponse> Handle(CreateFakturCommand request, CancellationToken cancellationToken)
        {
            //  GUARD
            Guard.Argument(() => request).NotNull()
                .Member(x => x.FakturDate, y => y.ValidDate("yyyy-MM-dd"))
                .Member(x => x.CustomerId, y => y.NotEmpty())
                .Member(x => x.SalesPersonId, y => y.NotEmpty())
                .Member(x => x.WarehouseId, y => y.NotEmpty())
                .Member(x => x.DueDate, y => y.ValidDate("yyyy-MM-dd"));

            //  BUILD
            _aggRoot = _builder
                .CreateNew(request)
                .FakturDate(request.FakturDate.ToDate(DateFormatEnum.YMD))
                .Customer(request)
                .SalesPerson(request)
                .Warehouse(request)
                .TglRencanaKirim(request.RencanaKirimDate.ToDate(DateFormatEnum.YMD))
                .User(request)
                .TermOfPayment((TermOfPaymentEnum)request.TermOfPayment)
                .DueDate(request.DueDate.ToDate(DateFormatEnum.YMD))
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
            using (var trans = TransHelper.NewScope())
            {
                _writer.Save(ref _aggRoot);
                //try
                //{
                //    GenStok();
                //}
                //catch (System.Exception)
                //{
                //    throw;
                //}
                trans.Complete();
            }

            _mediator.Publish(new CreatedFakturEvent(request, _aggRoot), cancellationToken);
            return Task.FromResult(GenResponse());
        }

        private async void GenStok()
        {
            foreach(var item in _aggRoot.ListItem)
            {
                var sat = item.ListQtyHarga.FirstOrDefault(x => x.Conversion == 1);
                var qty = item.ListQtyHarga.Sum(x => x.Qty * x.Conversion);
                var removeStok = new RemoveStokCommand(item.BrgId, _aggRoot.WarehouseId,
                    qty, sat.Satuan, item.HargaJual, _aggRoot.FakturId, "FAKTUR-JUAL");
            await _mediator.Send(removeStok);
            }
        }

        private CreateFakturResponse GenResponse()
        {
            var result = _aggRoot.Adapt<CreateFakturResponse>();
            return result;
        }
    }

    public class CreatedFakturEvent : INotification 
    {
        public CreatedFakturEvent(CreateFakturCommand command, FakturModel aggregate)
        {
            Command = command;
            Aggregate = aggregate;
        }
        public CreateFakturCommand Command { get; set; }
        public FakturModel Aggregate { get; set; }
    }

}