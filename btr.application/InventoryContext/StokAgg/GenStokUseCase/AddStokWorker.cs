using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Application;
using Dawn;

namespace btr.application.InventoryContext.StokAgg.GenStokUseCase
{
    public class AddStokRequest : IBrgKey, IWarehouseKey
    {
        public AddStokRequest(string brgId, string warehouseId, int qty, string satuan, decimal nilai, string reffId, string jenisMutasi)
        {
            BrgId = brgId;
            WarehouseId = warehouseId;
            Qty = qty;
            Satuan = satuan;
            Nilai = nilai;
            ReffId = reffId;
            JenisMutasi = jenisMutasi;
        }
        public string BrgId { get; set; }
        public string WarehouseId { get; set; }
        public int Qty { get; set; }
        public string Satuan { get; set; }
        public decimal Nilai { get; set; }
        public string ReffId { get; set; }
        public string JenisMutasi { get; set; }
    }
    public interface IAddStokWorker : INunaService<bool, AddStokRequest>
    {
    }

    public class AddStokWorker : IAddStokWorker
    {
        private StokModel _aggregate;
        private readonly IStokDal _stokDal;
        private readonly IStokBuilder _stokBuilder;
        private readonly IStokWriter _writer;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IStokBalanceBuilder _stokBalanceBuilder;
        private readonly IStokBalanceWriter _stokBalanceWriter;

        public AddStokWorker(IStokBuilder builder,
            IStokDal stokDal,
            IStokWriter writer,
            IBrgBuilder brgBuilder,
            IStokBalanceBuilder stokBalanceBuilder,
            IStokBalanceWriter stokBalanceWriter)
        {
            _stokBuilder = builder;
            _stokDal = stokDal;
            _writer = writer;
            _brgBuilder = brgBuilder;
            _stokBalanceBuilder = stokBalanceBuilder;
            _stokBalanceWriter = stokBalanceWriter;
        }

        public bool Execute(AddStokRequest request)
        {
            //  GUARD
            Guard.Argument(() => request).NotNull()
                .Member(x => x.BrgId, y => y.NotEmpty())
                .Member(x => x.WarehouseId, y => y.NotEmpty())
                .Member(x => x.Qty, y => y.GreaterThan(0))
                .Member(x => x.Satuan, y => y.NotEmpty());

            //  BUILD
            var konversi = GetKonversi(request, request.Satuan);
            var qtyKecil = request.Qty * konversi;
            var nilaiKecil = request.Nilai / konversi;
            _aggregate = _stokBuilder
                .Create(request, request, qtyKecil, nilaiKecil, request.ReffId, request.JenisMutasi)
                .Build();

            //  WRITE
            _writer.Save(_aggregate);

            //      stok balance
            var listStok = _stokDal.ListData(request, request) ??
                new List<StokModel>();
            var qtyBalance = listStok.Sum(x => x.Qty);
            var stokBalance = _stokBalanceBuilder
                .Load(request)
                .Qty(request, qtyBalance)
                .Build();
            _stokBalanceWriter.Save(ref stokBalance);
            return true;
        }

        private int GetKonversi(IBrgKey brgKey, string satuan)
        {
            var brg = _brgBuilder.Load(brgKey).Build();
            var thisSatuan = brg.ListSatuan.FirstOrDefault(x => x.Satuan.ToLower() == satuan.ToLower())
                ?? throw new KeyNotFoundException("Satuan invalid");
            return thisSatuan.Conversion;
        }
    }
}