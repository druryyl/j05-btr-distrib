using System;
using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Application;
using Polly;
using Polly.Fallback;

namespace btr.application.InventoryContext.StokAgg.GenStokUseCase
{
    public class GenStokFakturRegenRequest : IFakturKey
    {
        public GenStokFakturRegenRequest(string fakturId) => FakturId = fakturId;
        public string FakturId { get; set; }
    }

    public interface IGenStokFakturRegenWorker : INunaServiceVoid<GenStokFakturRequest>
    {
    }
    public class GenStokFakturRegenWorker : IGenStokFakturRegenWorker
    {
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IRollBackStokWorker _rollBackStokWorker;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IRemoveFifoStokWorker _removeFifoStokWorker;
        private readonly IAddStokWorker _addStokWorker;
        private readonly FallbackPolicy<bool> _fallbackExReturnFalse;

        public GenStokFakturRegenWorker(IFakturBuilder fakturBuilder, 
            IRollBackStokWorker rollBackStokWorker, 
            IBrgBuilder brgBuilder, IRemoveFifoStokWorker removeFifoStokWorker, 
            IAddStokWorker addStokWorker)
        {
            _fakturBuilder = fakturBuilder;
            _rollBackStokWorker = rollBackStokWorker;
            _brgBuilder = brgBuilder;
            _removeFifoStokWorker = removeFifoStokWorker;
            _addStokWorker = addStokWorker;
            
            _fallbackExReturnFalse = Policy<bool>
                .Handle<KeyNotFoundException>()
                .Or<ArgumentException>()
                .Fallback(() => false);
            
        }

        public void Execute(GenStokFakturRequest req)
        {
            var faktur = _fakturBuilder.Load(req).Build();

            if (!faktur.IsVoid)
                ExecuteGenStok(faktur);
            else
                ExecuteVoid(faktur);
        }
        
        private void ExecuteGenStok(FakturModel faktur)
        {
            using (var trans = TransHelper.NewScope())
            {
                _rollBackStokWorker.Execute(new RollBackStokRequest(faktur.FakturId));
                foreach(var item in faktur.ListItem)
                {
                    var brg = _brgBuilder.Load(item).Build();
                    var satuan = brg.ListSatuan.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty;

                    if (item.QtyJual != 0)
                        RemoveStok(item.BrgId, item.QtyJual, satuan, item.HrgSat, faktur);

                    var qtyBonus = item.QtyPotStok - item.QtyJual;
                    if (qtyBonus == 0)
                        continue;
                    RemoveStok(item.BrgId, qtyBonus, satuan, 0, faktur);
                }
                trans.Complete();
            }
        }
        private void ExecuteVoid(IFakturKey req)
        {
            var rollBackReq = new RollBackStokRequest(req.FakturId);
            _rollBackStokWorker.Execute(rollBackReq);
        }

        private void RemoveStok(string brgId, int qty, string satuan, decimal hrgsat, FakturModel faktur)
        {
            var reqRemoveStok = new RemoveFifoStokRequest(brgId,
                faktur.WarehouseId, qty, satuan, hrgsat, faktur.FakturId, "FAKTUR", faktur.CustomerName, faktur.FakturDate);
            var isSuccess = _fallbackExReturnFalse.Execute(() =>
            {
                _removeFifoStokWorker.Execute(reqRemoveStok);
                return true;
            });

            if (isSuccess)
                return;
            
            //  jika gagal regen, maka add stok dulu baru remove            
            AddStok(brgId, qty, satuan, hrgsat, faktur);
            _removeFifoStokWorker.Execute(reqRemoveStok);
        }
        
        private void AddStok(string brgId, int qty, string satuan, decimal hrgsat, FakturModel faktur)
        {
            var reffId = $"FAKTUR-REGEN";
            _addStokWorker.Execute(new AddStokRequest(brgId, faktur.WarehouseId, qty, satuan, hrgsat, "FAKTUR-REGEN", "ADJUST", faktur.FakturId, faktur.FakturDate));
        }
    }
}
