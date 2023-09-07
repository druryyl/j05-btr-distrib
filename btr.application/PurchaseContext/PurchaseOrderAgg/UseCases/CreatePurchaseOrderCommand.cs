using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using btr.application.PurchaseContext.PurchaseOrderAgg.Workers;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.PurchaseContext.PurchaseOrderAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using Dawn;

namespace btr.application.PurchaseContext.PurchaseOrderAgg.UseCases
{
    public class CreatePurchaseOrderCommand : IRequest<CreatePurchaseOrderResponse>,
        ISupplierKey, IWarehouseKey
    
    {
        public string SupplierId { get; set; }
        public string WarehouseId { get; set; }
        public decimal DiscountLain { get; set; }
        public decimal BiayaLain { get; set; }
        public List<CreatePurchaseOrderCommandItem> ListItem { get; set; }
    }

    public class CreatePurchaseOrderCommandItem : IBrgKey
    {
        public string BrgId { get; set; }
        public int Qty { get; set; }
        public string  Satuan { get; set; }
        public decimal Harga { get; set; }
        public decimal Diskon { get; set; }
        public decimal Tax { get; set; }
    }

    public class CreatePurchaseOrderResponse
    {
        public string PurchaseOrderId { get; set; }
    }

    public class CreatePurchaseOrderHandler : IRequestHandler<CreatePurchaseOrderCommand, CreatePurchaseOrderResponse>
    {
        private PurchaseOrderModel _aggRoot = new PurchaseOrderModel();
        private readonly IPurchaseOrderBuilder _builder;
        private readonly IPurchaseOrderWriter _writer;

        public CreatePurchaseOrderHandler(IPurchaseOrderBuilder builder, 
            IPurchaseOrderWriter writer)
        {
            _builder = builder;
            _writer = writer;
        }

        public Task<CreatePurchaseOrderResponse> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            //  GUARD
            Guard.Argument(() => request).NotNull()
                .Member(x => x.SupplierId, y => y.NotEmpty())
                .Member(x => x.WarehouseId, y => y.NotEmpty());
            
            //  BUILD
            _aggRoot = _builder
                .Create()
                .Supplier(request)
                .Warehouse(request)
                .Build();
            foreach (var item in request.ListItem)
                _aggRoot = _builder
                    .Attach(_aggRoot)
                    .AddItem(item, item.Qty, item.Satuan, item.Harga, item.Diskon, item.Tax)
                    .Build();

            //  WRITE
            _writer.Save(ref _aggRoot);
             
            //  RESPONSE
            return Task.FromResult(GenResponse());
        }

        private CreatePurchaseOrderResponse GenResponse()
        {
            return new CreatePurchaseOrderResponse
            {
                PurchaseOrderId = _aggRoot.PurchaseOrderId
            };
        }
    }

}

























