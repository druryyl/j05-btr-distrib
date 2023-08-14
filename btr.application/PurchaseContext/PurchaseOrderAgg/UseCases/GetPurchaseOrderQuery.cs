using btr.domain.PurchaseContext.PurchaseOrderAgg;
using MediatR;

namespace btr.application.PurchaseContext.PurchaseOrderAgg.UseCases
{
    public class GetPurchaseOrderQuery : IRequest<PurchaseOrderModel>
    {
    }

}
