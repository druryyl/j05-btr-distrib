using btr.application.PurchaseContext.SupplierAgg.Workers;
using btr.domain.PurchaseContext.SupplierAgg;
using Dawn;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace btr.application.PurchaseContext.SupplierAgg.UseCases
{
    public class GetSupplierQuery : IRequest<SupplierModel>,
        ISupplierKey
    {
        public GetSupplierQuery(string supplierId) => SupplierId = supplierId;
        public string SupplierId { get; set; }
    }

    public class GetSupplierHandler : IRequestHandler<GetSupplierQuery, SupplierModel>
    {
        private SupplierModel _agg;
        private readonly ISupplierBuilder _builder;

        public GetSupplierHandler(ISupplierBuilder builder)
        {
            _builder = builder;
        }

        public Task<SupplierModel> Handle(GetSupplierQuery request, CancellationToken cancellationToken)
        {
            //  GUARD
            Guard.Argument(() => request).NotNull()
                .Member(x => x.SupplierId, y => y.NotEmpty());

            //  QUERY
            _agg = _builder
                .Load(request)
                .Build();

            //  PROJECTION

            //  RESPONSE
            return Task.FromResult(_agg);
        }
    }

}
