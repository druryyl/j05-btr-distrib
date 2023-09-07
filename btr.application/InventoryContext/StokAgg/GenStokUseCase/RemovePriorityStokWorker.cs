using System;
using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Application;
using Dawn;

namespace btr.application.InventoryContext.StokAgg.GenStokUseCase
{
    public class RemovePriorityStokRequest : IBrgKey, IWarehouseKey
    {
        public RemovePriorityStokRequest(string brgId, string warehouseId, 
            int qty, string satuan, decimal hargaJual, string reffId, 
            string jenisMutasi, string priorityReffId)
        {
            BrgId = brgId;
            WarehouseId = warehouseId;
            Qty = qty;
            Satuan = satuan;
            HargaJual = hargaJual;
            ReffId = reffId;
            JenisMutasi = jenisMutasi;
            PriorityReffId = priorityReffId;
        }
        public string BrgId { get; set; }
        public string WarehouseId { get; set; }
        public int Qty { get; set; }
        public string Satuan { get; set; }
        public decimal HargaJual { get; set; }
        public string ReffId { get; set; }
        public string JenisMutasi { get; set; }
        public string PriorityReffId { get; }
    }

    public interface IRemovePriorityStokWorker : INunaServiceVoid<RemovePriorityStokRequest>
    { }
    public class RemovePriorityStokWorker : IRemovePriorityStokWorker
    {
        private readonly IStokDal _stokDal;
        private readonly IStokBuilder _stokBuilder;
        private readonly IStokWriter _stokWriter;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IGenStokBalanceWorker _stokBalanceWorker;

        public RemovePriorityStokWorker(IStokBuilder builder,
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

        public void Execute(RemovePriorityStokRequest request)
        {
            //  GUARD
            Guard.Argument(() => request).NotNull()
                .Member(x => x.BrgId, y => y.NotEmpty())
                .Member(x => x.WarehouseId, y => y.NotEmpty())
                .Member(x => x.Qty, y => y.GreaterThan(0))
                .Member(x => x.Satuan, y => y.NotEmpty());

            //  BUILD
            var konversi = GetKonversi(request, request.Satuan, out var brg);
            var qtyKecil = request.Qty * konversi;
            var hargaKecil = request.HargaJual / konversi;

            var listStokAll = _stokDal.ListData(request, request)?.ToList()
                ?? throw new KeyNotFoundException("Stok not found");
            //      reff-id yang diprioritaskan taro di atas
            var listStok = listStokAll.Where(x => x.ReffId == request.PriorityReffId).ToList();
            listStok.AddRange(listStokAll
                .Where(x => x.ReffId != request.PriorityReffId)
                .OrderBy(y => y.StokDate));
                
            int sisa = qtyKecil;
            var listMovingStok = new List<StokModel>();
            while (sisa > 0)
            {
                var stok = listStok
                    .FirstOrDefault(x => x.Qty > 0)
                    ?? throw new ArgumentException($"Stok tidak mencukupi: {brg.BrgName}");

                var pengurang = sisa >= qtyKecil ? qtyKecil : sisa;
                stok = _stokBuilder
                    .Load(stok)
                    .RemoveStok(pengurang, hargaKecil, request.ReffId, request.JenisMutasi)
                    .Build();

                listStok.RemoveAll(x => x.StokId == stok.StokId);
                listMovingStok.Add(stok);
                sisa -= pengurang;
            }

            //  WRITE
            using (var trans = TransHelper.NewScope())
            {
                //  stok
                foreach (var item in listMovingStok)
                    _stokWriter.Save(item);

                //  stok balance
                var stokBalanceReq = new GenStokBalanceRequest(request.BrgId, request.WarehouseId);
                _stokBalanceWorker.Execute(stokBalanceReq);

                trans.Complete();
            }

        }

        private int GetKonversi(IBrgKey brgKey, string satuan, out BrgModel brg)
        {
            brg = _brgBuilder.Load(brgKey).Build();
            var thisSatuan = brg.ListSatuan.FirstOrDefault(x => x.Satuan.ToLower() == satuan.ToLower())
                             ?? throw new KeyNotFoundException($"Satuan {brg.BrgName} invalid : {satuan}");
            return thisSatuan.Conversion;
        }

    }
}