using System;
using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.application.InventoryContext.StokBalanceAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Application;
using Dawn;

namespace btr.application.InventoryContext.StokAgg
{
    public class RemoveFifoStokRequest : IBrgKey, IWarehouseKey
    {
        public RemoveFifoStokRequest(string brgId, string warehouseId, int qty, string satuan, decimal hargaJual, string reffId, string jenisMutasi)
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

    public interface IRemoveFifoStokWorker : INunaServiceVoid<RemoveFifoStokRequest>
    { }
    public class RemoveFifoStokWorker : IRemoveFifoStokWorker
    {
        private readonly IStokDal _stokDal;
        private readonly IStokBuilder _stokBuilder;
        private readonly IStokWriter _stokWriter;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IGenStokBalanceWorker _stokBalanceWorker;

        public RemoveFifoStokWorker(IStokBuilder builder,
            IStokDal stokDal,
            IStokWriter writer, IBrgBuilder brgBuilder,
            IGenStokBalanceWorker stokBalanceWorker)
        {
            _stokDal = stokDal;
            _stokBuilder = builder;
            _stokWriter = writer;
            _brgBuilder = brgBuilder;
            _stokBalanceWorker = stokBalanceWorker;
        }

        public void Execute(RemoveFifoStokRequest request)
        {
            //  GUARD
            Guard.Argument(() => request).NotNull()
                .Member(x => x.BrgId, y => y.NotEmpty())
                .Member(x => x.WarehouseId, y => y.NotEmpty())
                .Member(x => x.Qty, y => y.GreaterThan(0))
                .Member(x => x.Satuan, y => y.NotEmpty());

            //  BUILD
            var konversi = GetKonversi(request, request.Satuan, out BrgModel brg);
            var qtyKecil = request.Qty * konversi;
            var hargaKecil = request.HargaJual / konversi;

            var listStok = _stokDal.ListData(request, request)?.ToList()
                ?? throw new KeyNotFoundException("Stok not found");
            int sisa = qtyKecil;
            var listMovingStok = new List<StokModel>();
            while (sisa > 0)
            {
                var stok = listStok
                    .OrderBy(x => x.StokId)
                    .FirstOrDefault(x => x.Qty > 0)
                    ?? throw new ArgumentException($"Stok tidak mencukupi: {brg.BrgName}");

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
                    _stokWriter.Save(ref anItem);
                }
                trans.Complete();
            }

            //  stok balance
            var stokBalanceReq = new GenStokBalanceRequest(request.BrgId, request.WarehouseId);
            _stokBalanceWorker.Execute(stokBalanceReq);
        }

        public int GetKonversi(IBrgKey brgKey, string satuan, out BrgModel brg)
        {
            brg = _brgBuilder.Load(brgKey).Build();
            var thisSatuan = brg.ListSatuan.FirstOrDefault(x => x.Satuan.ToLower() == satuan.ToLower())
                             ?? throw new KeyNotFoundException($"Satuan {brg.BrgName} invalid : {satuan}");
            return thisSatuan.Conversion;
        }

    }
}