using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.StokAgg;
using btr.application.InventoryContext.StokAgg.UseCases;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using Dawn;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace btr.application.SalesContext.FakturAgg.Workers
{
    public class SaveFakturRequest : IFakturKey, ICustomerKey, ISalesPersonKey, IWarehouseKey, IUserKey
    {
        public string FakturId { get; set; }
        public string FakturDate { get; set; }
        public string CustomerId { get; set; }
        public string SalesPersonId { get; set; }
        public string WarehouseId { get; set; }
        public string RencanaKirimDate { get; set; }
        public int TermOfPayment { get; set; }
        public string DueDate { get; set; }
        public string UserId { get; set; }
        public IEnumerable<SaveFakturRequestItem> ListBrg { get; set; }
    }

    public class SaveFakturRequestItem : IBrgKey
    {
        public string BrgId { get; set; }
        public string QtyString { get; set; }
        public string DiscountString { get; set; }
        public decimal PpnProsen { get; set; }
    }

    public interface ISaveFakturWorker : INunaService<FakturModel, SaveFakturRequest> { }

    public class SaveFakturWorker : ISaveFakturWorker
    {
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IFakturWriter _fakturWriter;
        private readonly IAddStokWorker _addStokWorker;
        private readonly IStokMutasiDal _stokMutasiDal;
        private readonly IRollBackStokWorker _rollBackStokWorker;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IRemoveFifoStokWorker _removeStokWorker;
        private readonly IMediator _mediator;

        public SaveFakturWorker(IFakturBuilder fakturBuilder,
            IFakturWriter fakturWriter,
            IAddStokWorker addStokWorker,
            IStokMutasiDal stokMutasiDal,
            IRollBackStokWorker rollBackStokWorker,
            IBrgBuilder brgBuilder,
            IRemoveFifoStokWorker removeStokWorker,
            IMediator mediator)
        {
            _fakturBuilder = fakturBuilder;
            _fakturWriter = fakturWriter;
            _addStokWorker = addStokWorker;
            _stokMutasiDal = stokMutasiDal;
            _rollBackStokWorker = rollBackStokWorker;
            _brgBuilder = brgBuilder;
            _removeStokWorker = removeStokWorker;
            _mediator = mediator;
        }

        public FakturModel Execute(SaveFakturRequest req)
        {
            //  GUARD
            Guard.Argument(() => req).NotNull()
                .Member(x => x.FakturDate, y => y.ValidDate("yyyy-MM-dd"))
                .Member(x => x.CustomerId, y => y.NotEmpty())
                .Member(x => x.SalesPersonId, y => y.NotEmpty())
                .Member(x => x.WarehouseId, y => y.NotEmpty())
                .Member(x => x.DueDate, y => y.ValidDate("yyyy-MM-dd"));

            //  PROSES FAKTUR
            var faktur = SaveFaktur(req);
            //GenStok(faktur);

            //  RESULT
            return faktur;
        }
        private FakturModel SaveFaktur(SaveFakturRequest req)
        {
            //  BUILD
            FakturModel result;
            if (req.FakturId.Length == 0)
            {
                result = _fakturBuilder.CreateNew(req).Build();
            }
            else
            {
                result = _fakturBuilder.Load(req).Build();
                result.ListItem.Clear();
            }

            result = _fakturBuilder
                .Attach(result)
                .FakturDate(req.FakturDate.ToDate(DateFormatEnum.YMD))
                .Customer(req)
                .SalesPerson(req)
                .Warehouse(req)
                .TglRencanaKirim(req.RencanaKirimDate.ToDate(DateFormatEnum.YMD))
                .User(req)
                .TermOfPayment((TermOfPaymentEnum)req.TermOfPayment)
                .DueDate(req.DueDate.ToDate(DateFormatEnum.YMD))
                .Build();

            foreach (var item in req.ListBrg)
            {
                result = _fakturBuilder
                    .Attach(result)
                    .AddItem(item, item.QtyString, item.DiscountString, item.PpnProsen)
                    .Build();
            }
            result = _fakturBuilder
                .Attach(result)
                .CalcTotal()
                .Build();

            //  APPLY
            _fakturWriter.Save(ref result);
            _mediator.Publish(new SavedFakturEvent(req, result));
            return result;
        }

        #region GENERATE-STOK
        private void GenStok(FakturModel faktur)
        {
            _rollBackStokWorker.Execute(new StokModel { ReffId = faktur.FakturId});
            foreach(var item in faktur.ListItem)
            {
                var brg = _brgBuilder.Load(item).Build();
                var satuan = brg.ListSatuan.FirstOrDefault(x => x.Conversion == 1)?.Satuan ?? string.Empty;
                GetQtyJual(item, out int qtyJual, out decimal harga);
                var req = new RemoveFifoStokRequest(item.BrgId,
                    faktur.WarehouseId, qtyJual, satuan, harga, faktur.FakturId, "FAKTUR");
                _removeStokWorker.Execute(req);

                GetQtyBonus(item, out int qtyBonus);
                if (qtyBonus == 0)
                    return;

                var reqBonus = new RemoveFifoStokRequest(item.BrgId,
                    faktur.WarehouseId, qtyBonus, satuan, 0, faktur.FakturId, "FAKTUR-BONUS");
                _removeStokWorker.Execute(reqBonus);
            }
        }

        private void GetQtyJual(FakturItemModel item, out int qtyKecil, out decimal hargaSat)
        {
            var item1= item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 1);
            var item2 = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 2);

            var qty1 = (item1?.Qty ?? 0) * (item1?.Conversion ?? 0);
            var qty2 = (item2?.Qty ?? 0) * (item2?.Conversion ?? 0);

            qtyKecil = qty1 + qty2;
            hargaSat = (item1?.HargaJual ?? 0) / (item1?.Conversion ?? 1);

            var item3 = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 3);
            var qty3 = (item3?.Qty ?? 0) * (item3?.Conversion ?? 0);
        }

        private void GetQtyBonus(FakturItemModel item, out int qty)
        {
            var item3 = item.ListQtyHarga.FirstOrDefault(x => x.NoUrut == 3);
            qty = (item3?.Qty ?? 0) * (item3?.Conversion ?? 0);
        }
        #endregion
    }

    public class SavedFakturEvent : INotification
    {
        public SavedFakturEvent(SaveFakturRequest request, FakturModel aggregate)
        {
            Request = request;
            Aggregate = aggregate;
        }
        public SaveFakturRequest Request { get; }
        public FakturModel Aggregate { get; }
    }
}
