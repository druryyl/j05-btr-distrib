using btr.application.InventoryContext.StokAgg;
using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.domain.InventoryContext.StokBalanceAgg;
using btr.domain.PurchaseContext.ReturBeliFeature;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

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
        private readonly IStokBuilder _stokBuilder;
        private readonly IStokWriter _stokWriter;
        private readonly IStokMutasiDal _stokMutasiDal;
        private readonly IStokBalanceBuilder _stokBalanceBuilder;
        private readonly IStokBalanceWriter _stokBalanceWriter;
        private readonly IGenStokBalanceWorker _genStokBalance;

        private readonly IRemoveStokEditReturBeliWorker _removeStokReturBeliWorker;

        public VoidReturBeliWorker(IReturBeliBuilder builder,
            IReturBeliWriter returBeliWriter,
            IRemoveStokEditReturBeliWorker removeStokReturBeliWorker,
            IStokBuilder stokBuilder,
            IStokWriter stokWriter,
            IStokMutasiDal stokMutasiDal,
            IStokBalanceBuilder stokBalanceBuilder,
            IStokBalanceWriter stokBalanceWriter,
            IGenStokBalanceWorker genStokBalance)
        {
            _builder = builder;
            _returBeliWriter = returBeliWriter;
            _removeStokReturBeliWorker = removeStokReturBeliWorker;
            _stokBuilder = stokBuilder;
            _stokWriter = stokWriter;
            _stokMutasiDal = stokMutasiDal;
            _stokBalanceBuilder = stokBalanceBuilder;
            _stokBalanceWriter = stokBalanceWriter;
            _genStokBalance = genStokBalance;
        }

        public void Execute(VoidReturBeliRequest req)
        {
            //  void returBeli
            var returBeli = _builder
                .Load(req)
                .Void(req)
                .Build();

            IReffKey reffKey = new StokModel { ReffId = req.ReturBeliId };
            var listStokMutasi = _stokMutasiDal.ListData(reffKey)?.ToList()
                ?? new List<StokMutasiModel>();
            var listStokId = listStokMutasi.Select(x => x.StokId).Distinct();
            var listStok = new List<StokModel>();
            var listBalanceStok = new List<StokBalanceModel>();
            foreach (var stokId in listStokId)
            {
                var stok = _stokBuilder
                    .Load(new StokModel { StokId = stokId })
                    .RollBack(reffKey)
                    .Build();
                listStok.Add(stok);
                var balance = _stokBalanceBuilder
                    .Load(new BrgModel{ BrgId = stok.BrgId })
                    .Qty(stok, stok.Qty)
                    .Build();
                listBalanceStok.Add(balance);
            }


            //  apply database
            using (var trans = TransHelper.NewScope())
            {
                foreach(var item in listStok)
                    _stokWriter.Save(item);

                foreach (var item2 in listBalanceStok)
                {
                    foreach(var item3 in item2.ListWarehouse)
                    {
                        var cmd = new GenStokBalanceRequest(item2.BrgId, item3.WarehouseId);
                        _genStokBalance.Execute(cmd);
                    }
                }

                _ = _returBeliWriter.Save(returBeli);
                trans.Complete();
            }
        }
    }
}
