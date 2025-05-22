using btr.application.BrgContext.BrgAgg;
using btr.application.PurchaseContext.ReturBeliAgg;
using btr.domain.PurchaseContext.ReturBeliFeature;
using btr.nuna.Application;
using System.Linq;

namespace btr.application.InventoryContext.StokAgg.GenStokUseCase
{
    public class GenStokReturBeliRequest : IReturBeliKey
    {
        public GenStokReturBeliRequest(string returBeliId) => ReturBeliId = returBeliId;
        public string ReturBeliId { get; set; }
    }

    public interface IGenStokReturBeliWorker : INunaServiceVoid<GenStokReturBeliRequest>
    {
    }
    public class GenStokReturBeliWorker : IGenStokReturBeliWorker
    {
        private readonly IReturBeliBuilder _returBeliBuilder;
        private readonly IRollBackStokWorker _rollBackStokWorker;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IRemoveFifoStokWorker _removeFifoStokWorker;


        public GenStokReturBeliWorker(IReturBeliBuilder returBeliBuilder,
            IBrgBuilder brgBuilder,
            IRollBackStokWorker rollBackStokWorker,
            IRemoveFifoStokWorker removeFifoStokWorker)
        {
            _returBeliBuilder = returBeliBuilder;
            _rollBackStokWorker = rollBackStokWorker;
            _brgBuilder = brgBuilder;
            _removeFifoStokWorker = removeFifoStokWorker;
        }

        public void Execute(GenStokReturBeliRequest req)
        {
            var returBeli = _returBeliBuilder.Load(req).Build();

            //if (!returBeli.IsVoid)
            //    ExecuteGenStok(returBeli);
            //else
            //    ExecuteVoid(returBeli);
        }

        //private void ExecuteGenStok(ReturBeliModel returBeli)
        //{
        //    using (var trans = TransHelper.NewScope())
        //    {
        //        _rollBackStokWorker.Execute(new RollBackStokRequest(returBeli.ReturBeliId));
        //        foreach (var item in returBeli.ListItem)
        //        {
        //            var brg = _brgBuilder.Load(item).Build();
        //            var satuan = brg.ListSatuan.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty;

        //            if (item. QtyJual != 0)
        //            {
        //                var reqRemoveStok = new RemoveFifoStokRequest(item.BrgId,
        //                    returBeli.WarehouseId, item.QtyJual, satuan, item.HrgSat, returBeli.ReturBeliId, "RETUR-BELI", returBeli.CustomerName, returBeli.ReturBeliDate);
        //                _removeFifoStokWorker.Execute(reqRemoveStok);
        //            }

        //            var qtyBonus = item.QtyPotStok - item.QtyJual;
        //            if (qtyBonus == 0)
        //                continue;

        //            var reqBonus = new RemoveFifoStokRequest(item.BrgId,
        //                returBeli.WarehouseId, qtyBonus, satuan, 0, returBeli.ReturBeliId, "FAKTUR-BONUS", returBeli.CustomerName, returBeli.ReturBeliDate);
        //            _removeFifoStokWorker.Execute(reqBonus);
        //        }
        //        trans.Complete();
        //    }
        //}
        //private void ExecuteVoid(IReturBeliKey req)
        //{
        //    var rollBackReq = new RollBackStokRequest(req.ReturBeliId);
        //    _rollBackStokWorker.Execute(rollBackReq);
        //}
    }
}
