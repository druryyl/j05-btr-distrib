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
                    GetQtyJual(item, out var qtyJual, out var harga);
                    var reqRemoveStok = new RemoveFifoStokRequest(item.BrgId,
                        faktur.WarehouseId, qtyJual, satuan, harga, faktur.FakturId, "FAKTUR");
                    _removeFifoStokWorker.Execute(reqRemoveStok);

                    GetQtyBonus(item, out var qtyBonus);
                    if (qtyBonus == 0)
                        break;

                    var reqBonus = new RemoveFifoStokRequest(item.BrgId,
                        faktur.WarehouseId, qtyBonus, satuan, 0, faktur.FakturId, "FAKTUR-BONUS");
                    _removeFifoStokWorker.Execute(reqBonus);
                }
                trans.Complete();
            }
        }
        
        #region HELPER
        private static void GetQtyJual(FakturItemModel item, out int qtyKecil, out decimal hargaSat)
        {
            var item1= item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 1);
            var item2 = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 2);

            var qty1 = (item1?.Qty ?? 0) * (item1?.Conversion ?? 0);
            var qty2 = (item2?.Qty ?? 0) * (item2?.Conversion ?? 0);

            qtyKecil = qty1 + qty2;
            hargaSat = (item1?.SubTotal ?? 0) / (item1?.Conversion ?? 1);
        }

        private static void GetQtyBonus(FakturItemModel item, out int qty)
        {
            var item3 = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 3);
            qty = (item3?.Qty ?? 0) * (item3?.Conversion ?? 0);
        }
        #endregion        
    }
}
