using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.domain.SalesContext.SalesPersonAgg;
using Mapster;
using MediatR;

namespace btr.application.SalesContext.SalesPersonAgg.UseCases
{
    public class ListDataSalesPersonQuery : IRequest<IEnumerable<ListDataSalesPersonResponse>>
    {
    }

    public class ListDataSalesPersonResponse
    {
        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
    }

    public class
        ListDataSalesPersonHandler : IRequestHandler<ListDataSalesPersonQuery, IEnumerable<ListDataSalesPersonResponse>>
    {
        private readonly ISalesPersonDal _salesPersonDal;

        public ListDataSalesPersonHandler(ISalesPersonDal salesPersonDal)
        {
            _salesPersonDal = salesPersonDal;
        }

        public Task<IEnumerable<ListDataSalesPersonResponse>> Handle(ListDataSalesPersonQuery request,
            CancellationToken cancellationToken)
        {
            //  BUILD
            var result = _salesPersonDal.ListData()
                         ?? throw new KeyNotFoundException("SalesPerson not found");

            //  APPLY
            return Task.FromResult(GenResponse(result));
        }

        private IEnumerable<ListDataSalesPersonResponse> GenResponse(IEnumerable<SalesPersonModel> listSalesPerson)
        {
            var result = listSalesPerson.Adapt<IEnumerable<ListDataSalesPersonResponse>>();
            return result;
        }
    }
}