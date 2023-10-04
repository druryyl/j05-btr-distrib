using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.PurchaseContext.InvoiceAgg;
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
        private readonly IRemovePriorityStokWorker _removePriorityStokWorker;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IAddStokWorker _addStokWorker;
        private readonly IStokDal _stokDal;
        private readonly IStokMutasiDal _stokMutasiDal;
        private readonly IRemoveRollbackStokWorker _removeRollbackStokWorker;

        public GenStokInvoiceWorker(IInvoiceBuilder invoiceBuilder,
            IRemovePriorityStokWorker removePriorityStokWorker,
            IBrgBuilder brgBuilder,
            IAddStokWorker addFifoStokWorker,
            IStokDal stokDal,
            IStokMutasiDal stokMutasiDal, 
            IRemoveRollbackStokWorker removeRollbackStokWorker)
        {
            _invoiceBuilder = invoiceBuilder;
            _removePriorityStokWorker = removePriorityStokWorker;
            _brgBuilder = brgBuilder;
            _addStokWorker = addFifoStokWorker;
            _stokDal = stokDal;
            _stokMutasiDal = stokMutasiDal;
            _removeRollbackStokWorker = removeRollbackStokWorker;
        }

        public void Execute(GenStokInvoiceRequest req)
        {
            var invoice = _invoiceBuilder.Load(req).Build();

            using (var trans = TransHelper.NewScope())
            {
                var reqRemove = new RemoveRollbackRequest(invoice.InvoiceId,"INVOICE-VOID");
                _removeRollbackStokWorker.Execute(reqRemove);
                
                foreach (var item in invoice.ListItem)
                {
                    var brg = _brgBuilder.Load(item).Build();
                    var satuan = brg.ListSatuan.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty;

                    var reqAddStok = new AddStokRequest(item.BrgId,
                        invoice.WarehouseId, item.QtyPotStok, satuan, item.HppSat, invoice.InvoiceId, "INVOICE");
                    _addStokWorker.Execute(reqAddStok);

                    var qtyBonus = item.QtyPotStok - item.QtyBeli;
                    if (qtyBonus == 0)
                        break;

                    var reqBonus = new AddStokRequest(item.BrgId,
                        invoice.WarehouseId, qtyBonus, satuan, 0, invoice.InvoiceId, "INVOICE-BONUS");
                    _addStokWorker.Execute(reqBonus);
                }
                trans.Complete();
            }
        }
    }
}
