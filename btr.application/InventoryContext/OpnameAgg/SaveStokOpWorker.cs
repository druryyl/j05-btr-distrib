using System;
using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.domain.InventoryContext.OpnameAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;

namespace btr.application.InventoryContext.OpnameAgg
{
    public class SaveStokOpRequest
    {
        public string StokOpId { get; set; }
        public string BrgId { get; set; }
        public string WarehouseId { get; set; }
        public string UserId { get; set; }
        public DateTime PeriodeOp { get; set; }
        public int QtyBesar { get; set; }
        public int QtyKecil { get; set; }
    }

    public interface ISaveStokOpWorker : INunaService<StokOpModel, SaveStokOpRequest>
    {
    }
    public class SaveStokOpWorker : ISaveStokOpWorker
    {
        private readonly IStokOpBuilder _builder;
        private readonly IStokOpWriter _writer;
        private readonly IGenStokStokOpWorker _genStokStokOpWorker;

        public SaveStokOpWorker(IStokOpBuilder builder,
            IStokOpWriter writer,
            IGenStokStokOpWorker genStokStokOpWorker)
        {
            _builder = builder;
            _writer = writer;
            _genStokStokOpWorker = genStokStokOpWorker;
        }

        public StokOpModel Execute(SaveStokOpRequest req)
        {

            var stokOp = req.StokOpId.Length == 0 
                ? _builder.Create(new StokOpModel { BrgId = req.BrgId, WarehouseId = req.WarehouseId }).Build() 
                : _builder.Load(new StokOpModel { StokOpId = req.StokOpId }).Build();

            stokOp = _builder
                .Attach(stokOp)
                .QtyOpname(req.QtyBesar, req.QtyKecil)
                .User(new UserModel(req.UserId))
                .Periode(req.PeriodeOp)
                .Build();

            StokOpModel result;
            using(var trans = TransHelper.NewScope())
            {
                result = _writer.Save(stokOp);
                var resultGenStok = _genStokStokOpWorker.Execute(new GenStokStokOpRequest(stokOp.StokOpId));
                //  update qty awal
                result = _builder
                    .Attach(result)
                    .QtyAwal(resultGenStok.QtyPcsAwal)
                    .Build();
                result = _writer.Save(result);
                
                trans.Complete();
            }
            return result;
        }
    }
}