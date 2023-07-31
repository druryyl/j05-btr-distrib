using System.Threading;
using System.Threading.Tasks;
using btr.application.SalesContext.SalesPersonAgg.Workers;
using btr.domain.SalesContext.SalesPersonAgg;
using Dawn;
using Mapster;
using MediatR;

namespace btr.application.SalesContext.SalesPersonAgg.UseCases
{
    public class GetSalesPersonQuery : IRequest<GetSalesPersonResponse>, ISalesPersonKey
    {
        public GetSalesPersonQuery(string id) => SalesPersonId = id;
        public string SalesPersonId { get; }
    }

    public class GetSalesPersonResponse
    {
        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
    }

    public class GetSalesPersonHandler : IRequestHandler<GetSalesPersonQuery, GetSalesPersonResponse>
    {
        private SalesPersonModel _aggRoot = new SalesPersonModel();
        private readonly ISalesPersonBuilder _builder;

        public GetSalesPersonHandler(ISalesPersonBuilder builder)
        {
            _builder = builder;
        }

        public Task<GetSalesPersonResponse> Handle(GetSalesPersonQuery request,
            CancellationToken cancellationToken)
        {
            //  GUARD
            Guard.Argument(() => request).NotNull()
                .Member(x => x.SalesPersonId, y => y.NotEmpty());

            //  BUILD
            _aggRoot = _builder
                .Load(request)
                .Build();

            //  APPLY
            return Task.FromResult(GenResponse());
        }

        private GetSalesPersonResponse GenResponse()
        {
            var result = _aggRoot.Adapt<GetSalesPersonResponse>();
            return result;
        }
    }
}