using btr.application.InventoryContext.StokAgg;
using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.application.SalesContext.FakturControlAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.FakturAgg.UseCases
{
    public class ReactivateFakturRequest : IFakturKey, IUserKey
    {
        public ReactivateFakturRequest(string fakturId, string userId)
        {
            FakturId = fakturId;
            UserId = userId;
        }
        public string FakturId { get; set; }
        public string UserId { get; set; }
    }

    public interface IReactivateFakturWorker : INunaServiceVoid<ReactivateFakturRequest>
    {
    }

    public class ReactivateFakturWorker : IReactivateFakturWorker
    {
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IRollBackStokWorker _rollBackStokWorker;
        private readonly IFakturControlBuilder _fakturControlBuilder;
        private readonly IFakturControlWriter _fakturControlWriter;
        private readonly IFakturWriter _fakturWriter;

        public ReactivateFakturWorker(IFakturBuilder fakturBuilder,
            IRollBackStokWorker rollBackStokWorker,
            IFakturControlBuilder fakturControlBuilder,
            IFakturControlWriter fakturControlWriter,
            IFakturWriter fakturWriter)
        {
            _fakturBuilder = fakturBuilder;
            _rollBackStokWorker = rollBackStokWorker;
            _fakturControlBuilder = fakturControlBuilder;
            _fakturControlWriter = fakturControlWriter;
            _fakturWriter = fakturWriter;
        }

        public void Execute(ReactivateFakturRequest req)
        {
            //   re-activate faktur
            var faktur = _fakturBuilder
                .Load(req)
                .ReActivate(req)
                .Build();
            //  unpost faktur control
            var fakturControl = _fakturControlBuilder
                .LoadOrCreate(req)
                .Posted(req)
                .Build();

            //  remove stok
            var rollBackReq = new RollBackStokRequest(req.FakturId);

            //  apply database
            using (var trans = TransHelper.NewScope())
            {
                _rollBackStokWorker.Execute(rollBackReq);
                _fakturWriter.Save(ref faktur);
                _fakturControlWriter.Save(fakturControl);
                trans.Complete();
            }

        }
    }
}
