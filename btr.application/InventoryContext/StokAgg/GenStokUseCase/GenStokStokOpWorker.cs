using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.OpnameAgg;
using btr.application.InventoryContext.ReturJualAgg.Workers;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.OpnameAgg;
using btr.domain.InventoryContext.ReturJualAgg;
using btr.domain.InventoryContext.StokBalanceAgg;
using btr.nuna.Application;

namespace btr.application.InventoryContext.StokAgg.GenStokUseCase
{
    public class GenStokStokOpRequest : IStokOpKey
    {
        public GenStokStokOpRequest(string id)
        {
            StokOpId = id;
        }

        public string StokOpId { get; set; }
    }

    public interface IGenStokStokOpWorker : INunaServiceVoid<GenStokStokOpRequest>
    {
    }
    
    public class GenStokStokOpWorker : IGenStokStokOpWorker
    {
        private readonly IStokOpBuilder _stokOpBuilder;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IAddStokWorker _addStokWorker;
        private readonly IRemoveFifoStokWorker _removeFifoStokWorker;
        private readonly IRollBackStokWorker _rollBackStokWorker;
        private readonly IStokBalanceWarehouseDal _stokBalanceWarehouseDal;

        public GenStokStokOpWorker(IStokOpBuilder stokOpBuilder,
            IBrgBuilder brgBuilder,
            IAddStokWorker addFifoStokWorker,
            IRemoveFifoStokWorker removeFifoStokWorker, 
            IRollBackStokWorker rollBackStokWorker, 
            IStokBalanceWarehouseDal stokBalanceWarehouseDal)
        {
            _stokOpBuilder = stokOpBuilder;
            _brgBuilder = brgBuilder;
            _addStokWorker = addFifoStokWorker;
            _removeFifoStokWorker = removeFifoStokWorker;
            _rollBackStokWorker = rollBackStokWorker;
            _stokBalanceWarehouseDal = stokBalanceWarehouseDal;
        }

        public void Execute(GenStokStokOpRequest req)
        {
            var stokOp = _stokOpBuilder.Load(req).Build();
            using (var trans = TransHelper.NewScope())
            {
                RollBackStokOp(stokOp);
                var qty = GetCurrentStok(stokOp);
                var qtyAdjust = CalcAdjustment(stokOp, qty);
                var brg = _brgBuilder.Load(stokOp).Build();
                var satuanKecil = brg.ListSatuan?.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty;
                var nilaiPersediaan = brg.Hpp;
                
                if (qtyAdjust > 0)
                {
                    var cmd = new AddStokRequest(stokOp.BrgId, stokOp.WarehouseId,
                        qtyAdjust, satuanKecil, nilaiPersediaan, stokOp.StokOpId, "STOKOP");
                    _addStokWorker.Execute(cmd);
                }
                else
                {
                    var cmd = new RemoveFifoStokRequest(stokOp.BrgId, stokOp.WarehouseId,
                        -qtyAdjust, satuanKecil, 0, stokOp.StokOpId, "STOKOP");
                    _removeFifoStokWorker.Execute(cmd);
                }
                trans.Complete();                    
            }    
        }
        private static int CalcAdjustment(StokOpModel stokOp, int qtyAwal)
        {
            var qtyAkhir = stokOp.QtyPcsOpname;
            var result = qtyAkhir - qtyAwal;
            return result;
        }

        private int GetCurrentStok(StokOpModel stokOp)
        {
            var listStok = _stokBalanceWarehouseDal.ListData((IBrgKey)stokOp)?.ToList()
                           ?? new List<StokBalanceWarehouseModel>();
            var result = listStok.FirstOrDefault(x => x.WarehouseId == stokOp.WarehouseId)?.Qty ?? 0;
            return result;
        }

        private void RollBackStokOp(IStokOpKey stokOp)
        {
            var req = new RollBackStokRequest(stokOp.StokOpId);
            _rollBackStokWorker.Execute(req);
        }
        
    }
}
