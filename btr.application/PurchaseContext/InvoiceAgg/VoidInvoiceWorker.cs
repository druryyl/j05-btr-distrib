using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.domain.PurchaseContext.InvoiceAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.PurchaseContext.InvoiceAgg
{
    public class VoidInvoiceRequest : IInvoiceKey, IUserKey
    {
        public VoidInvoiceRequest(string invoiceId, string userId)
        {
            InvoiceId = invoiceId;
            UserId = userId;
        }
        public string InvoiceId { get; set; }
        public string UserId { get; set; }
    }

    public interface IVoidInvoiceWorker : INunaServiceVoid<VoidInvoiceRequest>
    {
    }

    public class VoidInvoiceWorker : IVoidInvoiceWorker
    {
        private readonly IInvoiceBuilder _builder;
        private readonly IInvoiceWriter _invoiceWriter;
        private readonly IRemoveStokEditInvoiceWorker _removeStokInvoiceWorker;

        public VoidInvoiceWorker(IInvoiceBuilder builder,
            IInvoiceWriter invoiceWriter,
            IRemoveStokEditInvoiceWorker removeStokInvoiceWorker)
        {
            _builder = builder;
            _invoiceWriter = invoiceWriter;
            _removeStokInvoiceWorker = removeStokInvoiceWorker;
        }

        public void Execute(VoidInvoiceRequest req)
        {
            //  void invoice
            var invoice = _builder
                .Load(req)
                .Void(req)
                .Build();

            //  remove stok
            var removeReq = new RemoveStokEditInvoiceRequest(req.InvoiceId);

            //  apply database
            using (var trans = TransHelper.NewScope())
            {
                _removeStokInvoiceWorker.Execute(removeReq);
                _ = _invoiceWriter.Save(invoice);
                trans.Complete();
            }
        }
    }
}
