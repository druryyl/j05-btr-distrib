using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using Mapster;
using MediatR;

namespace btr.application.SalesContext.CustomerAgg.UseCases
{
    public class ListCustomerQuery : IRequest<IEnumerable<ListCustomerResponse>>
    {
    }

    public class ListCustomerResponse
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public double Plafond { get; set; }
        public double CreditBalance { get; set; }
    }

    public class ListCustomerHandler : IRequestHandler<ListCustomerQuery, IEnumerable<ListCustomerResponse>>
    {
        private readonly ICustomerDal _customerDal;

        public ListCustomerHandler(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }

        public Task<IEnumerable<ListCustomerResponse>> Handle(ListCustomerQuery request,
            CancellationToken cancellationToken)
        {
            //  BUILD
            var result = _customerDal.ListData()
                         ?? throw new KeyNotFoundException("Customer not found");

            //  APPLY
            return Task.FromResult(GenResponse(result));
        }

        private IEnumerable<ListCustomerResponse> GenResponse(IEnumerable<CustomerModel> listCustomer)
        {
            var result = listCustomer.Adapt<IEnumerable<ListCustomerResponse>>();
            return result;
        }
    }
}