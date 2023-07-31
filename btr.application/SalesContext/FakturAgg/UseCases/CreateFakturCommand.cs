using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.domain.InventoryContext.BrgAgg;
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
        public string FakturDate { get; }
        public string CustomerId { get; }
        public string SalesPersonId { get; }
        public string WarehouseId { get; }
        public string RencanaKirimDate { get; }
        public int TermOfPayment { get; }
        public string UserId { get; }
        public IEnumerable<CreateFakturCommandItem> ListBrg { get; }
    }

    public class CreateFakturCommandItem : IBrgKey
    {
        public string BrgId { get; }
        public string QtyString { get; }
        public string DiscountString { get; }
        public double PpnProsen { get; }
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

        public CreateFakturHandler(IFakturBuilder builder, IFakturWriter writer)
        {
            _builder = builder;
            _writer = writer;
        }

        public Task<CreateFakturResponse> Handle(CreateFakturCommand request, CancellationToken cancellationToken)
        {
            //  GUARD
            Guard.Argument(() => request).NotNull()
                .Member(x => x.FakturDate, y => y.ValidDate("yyyy-MM-dd"))
                .Member(x => x.CustomerId, y => y.NotEmpty())
                .Member(x => x.SalesPersonId, y => y.NotEmpty())
                .Member(x => x.WarehouseId, y => y.NotEmpty());

            //  BUILD
            _aggRoot = _builder
                .CreateNew(request)
                .FakturDate(request.FakturDate.ToDate(DateFormatEnum.YMD))
                .Customer(request)
                .SalesPerson(request)
                .Warehouse(request)
                .TglRencanaKirim(request.RencanaKirimDate.ToDate(DateFormatEnum.YMD))
                .Build();
            foreach (var item in request.ListBrg)
            {
                _aggRoot = _builder
                    .Attach(_aggRoot)
                    .AddItem(item, item.QtyString, item.DiscountString, item.PpnProsen)
                    .Build();
            }

            //  APPLY
            _writer.Save(ref _aggRoot);
            return Task.FromResult(GenResponse());
        }

        private CreateFakturResponse GenResponse()
        {
            var result = _aggRoot.Adapt<CreateFakturResponse>();
            return result;
        }
    }
}