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
        private readonly IRemovePriorityStokWorker _removePriorityStokWorker;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IAddStokWorker _addStokWorker;
        private readonly IStokDal _stokDal;
        private readonly IStokMutasiDal _stokMutasiDal;
        private readonly IStokBuilder _stokBuilder;

        public GenStokInvoiceWorker(IInvoiceBuilder invoiceBuilder,
            IRemovePriorityStokWorker removePriorityStokWorker,
            IBrgBuilder brgBuilder,
            IAddStokWorker addFifoStokWorker,
            IStokDal stokDal,
            IStokBuilder stokBuilder,
            IStokMutasiDal stokMutasiDal)
        {
            _invoiceBuilder = invoiceBuilder;
            _removePriorityStokWorker = removePriorityStokWorker;
            _brgBuilder = brgBuilder;
            _addStokWorker = addFifoStokWorker;
            _stokDal = stokDal;
            _stokBuilder = stokBuilder;
            _stokMutasiDal = stokMutasiDal;
        }

        public void Execute(GenStokInvoiceRequest req)
        {
            var invoice = _invoiceBuilder.Load(req).Build();

            using (var trans = TransHelper.NewScope())
            {
                RemoveStok(invoice);
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
        private void RemoveStok(IInvoiceKey invoiceKey)
        {
            var listStok = _stokDal.ListData(new StokModel { ReffId = invoiceKey.InvoiceId });
            if (listStok == null) 
                return;

            foreach(var item in listStok)
            {
                //  jika belum ada barang keluar, maka hapus
                if (item.Qty == item.QtyIn)
                {
                    _stokDal.Delete(item);
                    _stokMutasiDal.Delete(item);
                    continue;
                }

                //  jika sudah ada barang keluar, maka remove priority;
                var req = new RemovePriorityStokRequest(
                    item.BrgId, item.WarehouseId, item.Qty, string.Empty, 
                    item.NilaiPersediaan, invoiceKey.InvoiceId, "INVOICE-VOID", invoiceKey.InvoiceId);
                _removePriorityStokWorker.Execute(req);
            }
        }
    }
}
