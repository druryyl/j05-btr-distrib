using System.Threading;
using System.Threading.Tasks;
using btr.application.SalesContext.CustomerAgg.Workers;
using btr.domain.SalesContext.CustomerAgg;
using Dawn;
using Mapster;
using MediatR;

namespace btr.application.SalesContext.CustomerAgg.UseCases
{
    public class GetCustomerQuery : IRequest<GetCustomerResponse>, ICustomerKey
    {
        public GetCustomerQuery(string id) => CustomerId = id;
        public string CustomerId { get; }
    }

    public class GetCustomerResponse
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public double Plafond { get; set; }
        public double CreditBalance { get; set; }
    }

    public class GetCustomerHandler : IRequestHandler<GetCustomerQuery, GetCustomerResponse>
    {
        private CustomerModel _aggRoot = new CustomerModel();
        private readonly ICustomerBuilder _builder;

        public GetCustomerHandler(ICustomerBuilder builder)
        {
            _builder = builder;
        }

        public Task<GetCustomerResponse> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            //  GUARD
            Guard.Argument(() => request).NotNull()
                .Member(x => x.CustomerId, y => y.NotEmpty());

            //  BUILD
            _aggRoot = _builder
                .Load(request)
                .Build();

            //  APPLY
            return Task.FromResult(GenResponse());
        }

        private GetCustomerResponse GenResponse()
        {
            var result = _aggRoot.Adapt<GetCustomerResponse>();
            return result;
        }
    }
}