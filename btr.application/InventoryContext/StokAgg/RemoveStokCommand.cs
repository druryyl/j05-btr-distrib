using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using btr.application.BrgContext.BrgAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Application;
using Dawn;
using MediatR;

namespace btr.application.InventoryContext.StokAgg
{
    public class RemoveStokCommand : IRequest, IBrgKey, IWarehouseKey
    {
        public RemoveStokCommand(string brgId, string warehouseId, int qty, string satuan, decimal hargaJual, string reffId, string jenisMutasi)
        {
            BrgId = brgId;
            WarehouseId = warehouseId;
            Qty = qty;
            Satuan = satuan;
            HargaJual = hargaJual;
            ReffId = reffId;
            JenisMutasi = jenisMutasi;
        }
        public string BrgId { get; set; }
        public string WarehouseId { get; set; }
        public int Qty { get; set; }
        public string Satuan { get; set; }
        public decimal HargaJual { get; set; }
        public string ReffId { get; set; }
        public string JenisMutasi { get; set; }
    }

    public class RemoveStokHandler : IRequestHandler<RemoveStokCommand>
    {
        private StokModel _aggregate;
        private readonly IStokDal _stokDal;
        private readonly IStokBuilder _stokBuilder;
        private readonly IStokWriter _writer;
        private readonly IBrgBuilder _brgBuilder;

        public RemoveStokHandler(IStokBuilder builder,
            IStokDal stokDal,
            IStokWriter writer, IBrgBuilder brgBuilder)
        {
            _stokBuilder = builder;
            _stokDal = stokDal;
            _writer = writer;
            _brgBuilder = brgBuilder;
        }

        public Task Handle(RemoveStokCommand request, CancellationToken cancellationToken)
        {
            //  GUARD
            Guard.Argument(() => request).NotNull()
                .Member(x => x.BrgId, y => y.NotEmpty())
                .Member(x => x.WarehouseId, y => y.NotEmpty())
                .Member(x => x.Qty, y => y.GreaterThan(0))
                .Member(x => x.Satuan, y => y.NotEmpty());

            //  BUILD
            var brg = _brgBuilder.Load(request).Build();
            var listStok = _stokDal.ListData(request, request)?.ToList()
                ?? throw new KeyNotFoundException("Stok not found");
            var konversi = brg.ListSatuan
                .FirstOrDefault(x => x.Satuan == request.Satuan)?.Conversion
                ?? throw new KeyNotFoundException("Satuan invalid");
            var qtyKecil = request.Qty * konversi;
            var hargaKecil = request.HargaJual / konversi;

            var sisa = qtyKecil;
            var listMovingStok = new List<StokModel>();
            while(sisa > 0)
            {
                var stok = listStok
                    .OrderBy(x => x.StokId)
                    .FirstOrDefault(x => x.Qty > 0) 
                    ?? throw new ArgumentException($"Stok tidak mencukupi. {brg.BrgName}");

                var pengurang = sisa >= stok.Qty ? stok.Qty : sisa;
                stok = _stokBuilder
                    .Load(stok)
                    .RemoveStok(pengurang, request.HargaJual, request.ReffId, request.JenisMutasi)
                    .Build();

                listStok.RemoveAll(x => x.StokId == stok.StokId);
                listMovingStok.Add(stok);
                sisa -= pengurang;
            }

            //  WRITE
            using (var trans = TransHelper.NewScope())
            {
                foreach (var item in listMovingStok)
                {
                    var anItem = item;
                    _writer.Save(ref anItem);
                }
                trans.Complete();
            }

            return Task.FromResult(Unit.Value);
        }

        public decimal GetKonversi(IBrgKey brgKey, string satuan)
        {
            var brg = _brgBuilder.Load(brgKey).Build();
            var thisSatuan = brg.ListSatuan.FirstOrDefault(x => x.Satuan == satuan)
                             ?? throw new KeyNotFoundException("Satuan invalid");
            return thisSatuan.Conversion;
        }
    }
}