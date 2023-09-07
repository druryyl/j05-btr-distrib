using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.domain.PurchaseContext.SupplierAgg;
using MediatR;

namespace btr.application.PurchaseContext.SupplierAgg.UseCases
{
    public class ListSupplierQuery : IRequest<IEnumerable<SupplierModel>>
    {
    }

    public class ListSupplierHandler : IRequestHandler<ListSupplierQuery, IEnumerable<SupplierModel>>
    {
        private readonly ISupplierDal _supplierDal;

        public ListSupplierHandler(ISupplierDal supplierDal)
        {
            _supplierDal = supplierDal;
        }

        public Task<IEnumerable<SupplierModel>> Handle(ListSupplierQuery request, CancellationToken cancellationToken)
        {
            var result = _supplierDal.ListData()
                ?? throw new KeyNotFoundException("Supplier not found");
            return Task.FromResult(result);
        }
    }
}