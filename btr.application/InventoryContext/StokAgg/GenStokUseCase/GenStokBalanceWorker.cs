using btr.application.InventoryContext.StokBalanceAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Application;
using System.Collections.Generic;
using System.Linq;

namespace btr.application.InventoryContext.StokAgg.GenStokUseCase
{
    public class GenStokBalanceRequest : IBrgKey, IWarehouseKey
    {
        public GenStokBalanceRequest(string id, string warehouse)
        {
            BrgId = id;
            WarehouseId = warehouse;
        }
        public string BrgId { get; }
        public string WarehouseId { get; }
    }
    public interface IGenStokBalanceWorker : INunaServiceVoid<GenStokBalanceRequest>
    {
    }

    public class GenStokBalanceWorker : IGenStokBalanceWorker
    {
        private readonly IStokDal _stokDal;
        private readonly IStokBalanceBuilder _builder;
        private readonly IStokBalanceWriter _writer;

        public GenStokBalanceWorker(IStokDal stokDal,
            IStokBalanceBuilder stokBalanceBuilder,
            IStokBalanceWriter writer)
        {
            _stokDal = stokDal;
            _builder = stokBalanceBuilder;
            _writer = writer;
        }

        public void Execute(GenStokBalanceRequest request)
        {
            var listStok = _stokDal.ListData(request, request) ??
                new List<StokModel>();
            var qtyBalance = listStok.Sum(x => x.Qty);
            var stokBalance = _builder
                .Load(request)
                .Qty(request, qtyBalance)
                .Build();
            _writer.Save(ref stokBalance);
        }

    }
}
