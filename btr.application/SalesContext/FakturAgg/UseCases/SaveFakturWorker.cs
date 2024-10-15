using System;
using System.Collections.Generic;
using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using Dawn;
using MediatR;
using Newtonsoft.Json;
using Polly;

namespace btr.application.SalesContext.FakturAgg.UseCases
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
        public decimal Cash { get; set; }
        public string Note { get; set; }
        public IEnumerable<SaveFakturRequestItem> ListBrg { get; set; }
    }

    public class SaveFakturRequestItem : IBrgKey
    {
        public string BrgId { get; set; }
        public string StokHarga { get; set; }
        public string QtyString { get; set; }
        public string HrgString { get; set; }
        public string DiscountString { get; set; }
        public decimal PpnProsen { get; set; }
    }

    public interface ISaveFakturWorker : INunaService<FakturModel, SaveFakturRequest> { }

    public class SaveFakturWorker : ISaveFakturWorker
    {
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IFakturWriter _fakturWriter;
        private readonly IMediator _mediator;
        private readonly IGenStokFakturWorker _genStokWorker;

        public SaveFakturWorker(IFakturBuilder fakturBuilder,
            IFakturWriter fakturWriter,
            IMediator mediator, IGenStokFakturWorker genStokWorker)
        {
            _fakturBuilder = fakturBuilder;
            _fakturWriter = fakturWriter;
            _mediator = mediator;
            _genStokWorker = genStokWorker;
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
            FakturModel result;
            var fallback = Policy<FakturModel>
                .Handle<KeyNotFoundException>()
                .Fallback(() => null);
            var existingFaktur = fallback.Execute(() => _fakturBuilder.Load(req).Build());

            using (var trans = TransHelper.NewScope())
            {
                result = SaveFaktur(req);
                
                //      generate stok dilakukan jika data baru atau...
                //      edit faktur dan item berubah (jika hanya edit header,
                //      maka tidak usah gen stok
                var genStokReq = new GenStokFakturRequest(result.FakturId);
                if (existingFaktur is null)
                { 
                    _genStokWorker.Execute(genStokReq);
                    trans.Complete();
                    return result;
                }

                if (IsItemBerubah(existingFaktur, result))
                {
                    _genStokWorker.Execute(genStokReq);
                    trans.Complete();
                }
            }

            //  RESULT
            return result;
        }

        private static bool IsItemBerubah(FakturModel before, FakturModel after)
        {
            foreach (var item in before.ListItem)
            {
                item.DiscRp = Math.Round(item.DiscRp, 0);
                item.PpnRp = Math.Round(item.PpnRp, 0);
                item.Total = Math.Round(item.Total, 0);
            }
            var x = JsonConvert.SerializeObject(before.ListItem);

            foreach (var item in after.ListItem)
            {
                item.DiscRp = Math.Round(item.DiscRp, 0);
                item.PpnRp = Math.Round(item.PpnRp, 0);
                item.Total =  Math.Round(item.Total, 0);
            }
            var y = JsonConvert.SerializeObject(after.ListItem);
            var result = x != y;
            return result;
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
                .Cash(req.Cash)
                .Note(req.Note)
                .Build();

            foreach (var item in req.ListBrg)
            {
                result = _fakturBuilder
                    .Attach(result)
                    .AddItem(item, item.StokHarga, item.QtyString, item.HrgString, item.DiscountString, item.PpnProsen)
                    .Build();
            }
            result = _fakturBuilder
                .Attach(result)
                .CalcTotal()
                .Build();

            //  APPLY
            _ = _fakturWriter.Save(result);
            _mediator.Publish(new SavedFakturEvent(req, result));
            return result;
        }
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
