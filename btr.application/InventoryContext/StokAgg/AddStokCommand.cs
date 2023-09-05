using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using Dawn;
using MediatR;

namespace btr.application.InventoryContext.StokAgg
{
    public class AddStokCommand : IRequest, IBrgKey, IWarehouseKey
    {
        public AddStokCommand(string brgId, string warehouseId, int qty, string satuan, decimal nilai, string reffId, string jenisMutasi)
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

    public class AddStokHandler : IRequestHandler<AddStokCommand>
    {
        private StokModel _aggregate;
        private readonly IStokDal _stokDal;
        private readonly IStokBuilder _stokBuilder;
        private readonly IStokWriter _writer;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IStokBalanceBuilder _stokBalanceBuilder;
        private readonly IStokBalanceWriter _stokBalanceWriter;

        public AddStokHandler(IStokBuilder builder,
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

        public Task Handle(AddStokCommand request, CancellationToken cancellationToken)
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
                .Create(request, request, (int)qtyKecil, nilaiKecil, request.ReffId, request.JenisMutasi)
                .Build();

            //  WRITE
            _writer.Save(ref _aggregate);

            //      stok balance
            var listStok = _stokDal.ListData(request, request) ??
                new List<StokModel>();
            var qtyBalance = listStok.Sum(x => x.Qty);
            var stokBalance = _stokBalanceBuilder
                .Load(request)
                .Qty(request, qtyBalance)
                .Build();
            _stokBalanceWriter.Save(ref  stokBalance);
            return Task.FromResult(Unit.Value);
        }

        public decimal GetKonversi(IBrgKey brgKey, string satuan)
        {
            var brg = _brgBuilder.Load(brgKey).Build();
            var thisSatuan = brg.ListSatuan.FirstOrDefault(x => x.Satuan == satuan)
                             ?? throw new KeyNotFoundException("Satuan invalid");
            return thisSatuan.Conversion;
        }
        private int ConvertQtySatKecil(IBrgKey brgKey, int qty, string satuan)
        {
            var brg = _brgBuilder.Load(brgKey).Build();
            var thisSatuan = brg.ListSatuan.FirstOrDefault(x => x.Satuan == satuan)
                             ?? throw new KeyNotFoundException("Satuan invalid");
            var result = qty * thisSatuan.Conversion;
            return result;
        }
    }
}