using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.ReturJualAgg.Workers;
using btr.domain.InventoryContext.ReturJualAgg;
using btr.nuna.Application;

namespace btr.application.InventoryContext.StokAgg.GenStokUseCase
{
    public class GenStokReturJualRequest : IReturJualKey
    {
        public GenStokReturJualRequest(string id)
        {
            ReturJualId = id;
        }

        public string ReturJualId { get; set; }
    }

    public interface IGenStokReturJualWorker : INunaServiceVoid<GenStokReturJualRequest>
    {
    }
    
    public class GenStokReturJualWorker : IGenStokReturJualWorker
    {
        private readonly IReturJualBuilder _returJualBuilder;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IAddStokWorker _addStokWorker;
        private readonly IRemoveRollbackStokWorker _removeRollbackStokWorker;

        public GenStokReturJualWorker(IReturJualBuilder returJualBuilder,
            IBrgBuilder brgBuilder,
            IAddStokWorker addFifoStokWorker,
            IRemoveRollbackStokWorker removeRollbackStokWorker)
        {
            _returJualBuilder = returJualBuilder;
            _brgBuilder = brgBuilder;
            _addStokWorker = addFifoStokWorker;
            _removeRollbackStokWorker = removeRollbackStokWorker;
        }

        public void Execute(GenStokReturJualRequest req)
        {
            var returJual = _returJualBuilder.Load(req).Build();

            using (var trans = TransHelper.NewScope())
            {
                var reqRemove = new RemoveRollbackRequest(returJual.ReturJualId,"RETURJUAL-VOID");
                _removeRollbackStokWorker.Execute(reqRemove);
                
                foreach (var item in returJual.ListItem)
                {
                    var brg = _brgBuilder.Load(item).Build();
                    var satuan = brg.ListSatuan.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty;

                    var reqAddStok = new AddStokRequest(item.BrgId,
                        returJual.WarehouseId, item.Qty, satuan, brg.Hpp, returJual.ReturJualId, "RETURJUAL", returJual.CustomerName);
                    _addStokWorker.Execute(reqAddStok);
                }
                trans.Complete();
            }
        }
    }
}
