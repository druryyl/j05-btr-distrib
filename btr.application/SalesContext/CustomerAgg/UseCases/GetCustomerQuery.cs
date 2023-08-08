using System.Threading;
using System.Threading.Tasks;
using btr.application.SalesContext.CustomerAgg.Workers;
using btr.domain.SalesContext.CustomerAgg;
using Dawn;
using MediatR;

namespace btr.application.SalesContext.CustomerAgg.UseCases
{
    public class GetCustomerQuery : IRequest<CustomerModel>, ICustomerKey
    {
        public GetCustomerQuery(string id) => CustomerId = id;
        public string CustomerId { get; }
    }


    public class GetCustomerHandler : IRequestHandler<GetCustomerQuery, CustomerModel>
    {
        private CustomerModel _aggRoot = new CustomerModel();
        private readonly ICustomerBuilder _builder;

        public GetCustomerHandler(ICustomerBuilder builder)
        {
            _builder = builder;
        }

        public Task<CustomerModel> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            //  GUARD
            Guard.Argument(() => request).NotNull()
                .Member(x => x.CustomerId, y => y.NotEmpty());

            //  BUILD
            _aggRoot = _builder
                .Load(request)
                .Build();

            //  APPLY
            return Task.FromResult(_aggRoot);
        }
    }
}