using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace btr.application.PurchaseContext.PurchaseOrderAgg.UseCases
{
    public class CreatePurchaseOrderCommand : IRequest<CreatePurchaseOrderResponse>
    {
        public string SupplierId { get; set; }
        public string WarehouseId { get; set; }
        public List<CreatePurchaseOrderCommandItem> ListItem { get; set; }
    }

    public class CreatePurchaseOrderResponse
    {
        public string PurchaseOrderId { get; set; }
    }

    public class CreatePurchaseOrderHandler : IRequestHandler<CreatePurchaseOrderCommand, CreatePurchaseOrderResponse>
    {
        public Task<CreatePurchaseOrderResponse> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
