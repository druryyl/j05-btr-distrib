using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Application;

namespace btr.application.InventoryContext.StokAgg.GenStokUseCase
{
    public class GenStokFakturRequest : IFakturKey
    {
        public GenStokFakturRequest(string fakturId)
        {
            FakturId = fakturId;
        }

        public string FakturId { get; set; }
    }

    public interface IGenStokFakturWorker : INunaServiceVoid<GenStokFakturRequest>
    {
    }
    public class GenStokFakturWorker : IGenStokFakturWorker
    {
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IRollBackStokWorker _rollBackStokWorker;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IRemoveFifoStokWorker _removeFifoStokWorker;

        public GenStokFakturWorker(IFakturBuilder fakturBuilder, 
            IRollBackStokWorker rollBackStokWorker, 
            IBrgBuilder brgBuilder, IRemoveFifoStokWorker removeFifoStokWorker)
        {
            _fakturBuilder = fakturBuilder;
            _rollBackStokWorker = rollBackStokWorker;
            _brgBuilder = brgBuilder;
            _removeFifoStokWorker = removeFifoStokWorker;
        }

        public void Execute(GenStokFakturRequest req)
        {
            var faktur = _fakturBuilder.Load(req).Build();

            using (var trans = TransHelper.NewScope())
            {
                _rollBackStokWorker.Execute(new RollBackStokRequest(faktur.FakturId));
                foreach(var item in faktur.ListItem)
                {
                    var brg = _brgBuilder.Load(item).Build();
                    var satuan = brg.ListSatuan.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty;

                    if (item.QtyJual != 0)
                    {
                        var reqRemoveStok = new RemoveFifoStokRequest(item.BrgId,
                            faktur.WarehouseId, item.QtyJual, satuan, item.HrgSat, faktur.FakturId, "FAKTUR", faktur.CustomerName);
                        _removeFifoStokWorker.Execute(reqRemoveStok);
                    }

                    var qtyBonus = item.QtyPotStok - item.QtyJual;
                    if (qtyBonus == 0)
                        break;

                    var reqBonus = new RemoveFifoStokRequest(item.BrgId,
                        faktur.WarehouseId, qtyBonus , satuan, 0, faktur.FakturId, "FAKTUR-BONUS", faktur.CustomerName);
                    _removeFifoStokWorker.Execute(reqBonus);
                }
                trans.Complete();
            }
        }
    }
}
