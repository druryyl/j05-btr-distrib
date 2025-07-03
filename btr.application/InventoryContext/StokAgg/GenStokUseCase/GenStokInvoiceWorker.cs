using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.PurchaseContext.InvoiceAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.domain.PurchaseContext.InvoiceAgg;
using btr.nuna.Application;

namespace btr.application.InventoryContext.StokAgg.GenStokUseCase
{
    public class GenStokInvoiceRequest : IInvoiceKey
    {
        public GenStokInvoiceRequest(string invoiceId)
        {
            InvoiceId = invoiceId;
        }

        public string InvoiceId { get; set; }
    }

    public interface IGenStokInvoiceWorker : INunaServiceVoid<GenStokInvoiceRequest>
    {
    }
    public class GenStokInvoiceWorker : IGenStokInvoiceWorker
    {
        private readonly IInvoiceBuilder _invoiceBuilder;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IAddStokWorker _addStokWorker;
        private readonly IRemoveStokEditInvoiceWorker _removeStokEditInvoiceWorker;


        public GenStokInvoiceWorker(IInvoiceBuilder invoiceBuilder,
            IBrgBuilder brgBuilder,
            IAddStokWorker addFifoStokWorker,
            IRemoveStokEditInvoiceWorker removeStokEditInvoiceWorker)
        {
            _invoiceBuilder = invoiceBuilder;
            _brgBuilder = brgBuilder;
            _addStokWorker = addFifoStokWorker;
            _removeStokEditInvoiceWorker = removeStokEditInvoiceWorker;
        }

        public void Execute(GenStokInvoiceRequest req)
        {
            var invoice = _invoiceBuilder.Load(req).Build();

            using (var trans = TransHelper.NewScope())
            {
                var reqRemove = new RemoveStokEditInvoiceRequest(invoice.InvoiceId);
                _removeStokEditInvoiceWorker.Execute(reqRemove);
                
                foreach (var item in invoice.ListItem)
                {
                    var brg = _brgBuilder.Load(item).Build();
                    var satuan = brg.ListSatuan.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty;

                    if (item.QtyBeli != 0)
                    {
                        var reqAddStok = new AddStokRequest(item.BrgId,
                            invoice.WarehouseId, item.QtyBeli, satuan, item.HppSat, invoice.InvoiceId, "INVOICE", invoice.SupplierName, invoice.InvoiceDate);
                        _addStokWorker.Execute(reqAddStok);
                    }

                    var qtyBonus = item.QtyPotStok - item.QtyBeli;
                    if (qtyBonus == 0)
                        continue;

                    var reqBonus = new AddStokRequest(item.BrgId,
                        invoice.WarehouseId, qtyBonus, satuan, 0, invoice.InvoiceId, "INVOICE-BONUS", invoice.SupplierName, invoice.InvoiceDate);
                    _addStokWorker.Execute(reqBonus);
                }

                trans.Complete();
            }
        }
    }
}
