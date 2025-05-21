using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.PurchaseContext.ReturBeliAgg;
using btr.domain.PurchaseContext.ReturBeliFeature;
using btr.nuna.Application;

namespace btr.application.InventoryContext.StokAgg.GenStokUseCase
{
    public class GenStokReturBeliRequest : IReturBeliKey
    {
        public GenStokReturBeliRequest(string invoiceId)
        {
            ReturBeliId = invoiceId;
        }

        public string ReturBeliId { get; set; }
    }

    public interface IGenStokReturBeliWorker : INunaServiceVoid<GenStokReturBeliRequest>
    {
    }
    public class GenStokReturBeliWorker : IGenStokReturBeliWorker
    {
        private readonly IReturBeliBuilder _invoiceBuilder;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IAddStokWorker _addStokWorker;
        private readonly IRemoveStokEditReturBeliWorker _removeStokEditReturBeliWorker;
        private readonly IStokDal _stokDal;
        private readonly IStokBuilder _stokBuilder;
        private readonly IStokWriter _stokWriter;


        public GenStokReturBeliWorker(IReturBeliBuilder invoiceBuilder,
            IBrgBuilder brgBuilder,
            IAddStokWorker addFifoStokWorker,
            IRemoveStokEditReturBeliWorker removeStokEditReturBeliWorker,
            IStokDal stokDal,
            IStokBuilder stokBuilder,
            IStokWriter stokWriter)
        {
            _invoiceBuilder = invoiceBuilder;
            _brgBuilder = brgBuilder;
            _addStokWorker = addFifoStokWorker;
            _removeStokEditReturBeliWorker = removeStokEditReturBeliWorker;
            _stokDal = stokDal;
            _stokBuilder = stokBuilder;
            _stokWriter = stokWriter;
        }

        public void Execute(GenStokReturBeliRequest req)
        {
            var invoice = _invoiceBuilder.Load(req).Build();

            using (var trans = TransHelper.NewScope())
            {
                //  TODO: Harusnya di sini remove kembali stok yang tidak bisa di-rollback
                //        (setelah add stok invoice hasil edit)  
                var reqRemove = new RemoveStokEditReturBeliRequest(invoice.ReturBeliId);
                _removeStokEditReturBeliWorker.Execute(reqRemove);

                foreach (var item in invoice.ListItem)
                {
                    var brg = _brgBuilder.Load(item).Build();
                    var satuan = brg.ListSatuan.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty;

                    if (item.QtyBeli != 0)
                    {
                        var reqAddStok = new AddStokRequest(item.BrgId,
                            invoice.WarehouseId, item.QtyBeli, satuan, item.HppSat, invoice.ReturBeliId, "INVOICE", invoice.SupplierName, invoice.ReturBeliDate);
                        _addStokWorker.Execute(reqAddStok);
                    }

                    var qtyBonus = item.QtyPotStok - item.QtyBeli;
                    if (qtyBonus == 0)
                        continue;

                    var reqBonus = new AddStokRequest(item.BrgId,
                        invoice.WarehouseId, qtyBonus, satuan, 0, invoice.ReturBeliId, "INVOICE-BONUS", invoice.SupplierName, invoice.ReturBeliDate);
                    _addStokWorker.Execute(reqBonus);
                }

                //  TODO: nah di sini pross remove
                trans.Complete();
            }
        }
    }
}
