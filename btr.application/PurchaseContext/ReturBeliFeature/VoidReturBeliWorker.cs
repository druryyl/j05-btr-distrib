using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.domain.PurchaseContext.ReturBeliFeature;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;

namespace btr.application.PurchaseContext.ReturBeliAgg
{
    public class VoidReturBeliRequest : IReturBeliKey, IUserKey
    {
        public VoidReturBeliRequest(string returBeliId, string userId)
        {
            ReturBeliId = returBeliId;
            UserId = userId;
        }
        public string ReturBeliId { get; set; }
        public string UserId { get; set; }
    }

    public interface IVoidReturBeliWorker : INunaServiceVoid<VoidReturBeliRequest>
    {
    }

    public class VoidReturBeliWorker : IVoidReturBeliWorker
    {
        private readonly IReturBeliBuilder _builder;
        private readonly IReturBeliWriter _returBeliWriter;
        private readonly IRemoveStokEditReturBeliWorker _removeStokReturBeliWorker;

        public VoidReturBeliWorker(IReturBeliBuilder builder,
            IReturBeliWriter returBeliWriter,
            IRemoveStokEditReturBeliWorker removeStokReturBeliWorker)
        {
            _builder = builder;
            _returBeliWriter = returBeliWriter;
            _removeStokReturBeliWorker = removeStokReturBeliWorker;
        }

        public void Execute(VoidReturBeliRequest req)
        {
            //  void returBeli
            var returBeli = _builder
                .Load(req)
                .Void(req)
                .Build();

            //  remove stok
            var removeReq = new RemoveStokEditReturBeliRequest(req.ReturBeliId);

            //  apply database
            using (var trans = TransHelper.NewScope())
            {
                _removeStokReturBeliWorker.Execute(removeReq);
                _ = _returBeliWriter.Save(returBeli);
                trans.Complete();
            }
        }
    }
}
