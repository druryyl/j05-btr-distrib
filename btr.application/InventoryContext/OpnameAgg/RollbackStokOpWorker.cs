using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.domain.InventoryContext.OpnameAgg;
using btr.nuna.Application;

namespace btr.application.InventoryContext.OpnameAgg
{
    public class RollbackStokOpRequest : IStokOpKey
    {
        public RollbackStokOpRequest(string id) => StokOpId = id;

        public string StokOpId { get; set; }
    }

    public interface IRollbackStokOpWorker : INunaServiceVoid<RollbackStokOpRequest>
    {
    }

    public class RollbackStokOpWorker : IRollbackStokOpWorker
    {
        private readonly IStokOpBuilder _builder;
        private readonly IStokOpWriter _writer;
        private readonly IRollBackStokWorker _rollbackStokWorker;
        private readonly IRemovePriorityStokWorker _removePriorityStokWorker;
        private readonly IBrgBuilder _brgBuilder;

        public RollbackStokOpWorker(IStokOpBuilder builder,
            IRollBackStokWorker rollbackStokWorker,
            IRemovePriorityStokWorker removePriorityStokWorker,
            IStokOpWriter writer,
            IBrgBuilder brgBuilder)
        {
            _builder = builder;
            _rollbackStokWorker = rollbackStokWorker;
            _removePriorityStokWorker = removePriorityStokWorker;
            _writer = writer;
            _brgBuilder = brgBuilder;
        }

        public void Execute(RollbackStokOpRequest req)
        {
            var stokOp = _builder.Load(req).Build();
            using (var trans = TransHelper.NewScope())
            {
                RollBackStokOp(stokOp);
                _writer.Delete(stokOp);
                trans.Complete();
            }
        }

        private void RollBackStokOp(StokOpModel stokOp)
        {
            if (stokOp.QtyPcsAdjust == 0)
                return;

            if (stokOp.QtyPcsAdjust < 0)
                AddStok(stokOp);
            else
                RemoveStok(stokOp);
        }

        private void AddStok(IStokOpKey stokOp)
        {
            var req = new RollBackStokRequest(stokOp.StokOpId);
            _rollbackStokWorker.Execute(req);
        }

        private void RemoveStok(StokOpModel stokOp)
        {
            var brg = _brgBuilder.Load(stokOp).Build();
            var req = new RemovePriorityStokRequest(stokOp.BrgId, stokOp.WarehouseId,
                stokOp.QtyPcsAdjust,
                brg.ListSatuan.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty,
                0,
                stokOp.StokOpId,
                "STOKOP", stokOp.StokOpId,
                "Rollback StokOp", stokOp.StokOpDate);
            _removePriorityStokWorker.Execute(req);
        }
   }
}