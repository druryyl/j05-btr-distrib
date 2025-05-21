using System.Collections.Generic;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.PurchaseContext.ReturBeliFeature;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using Dawn;
using MediatR;
using Polly;

namespace btr.application.PurchaseContext.ReturBeliAgg
{
    public class SaveReturBeliRequest : IReturBeliKey, ISupplierKey,
        IWarehouseKey, IUserKey
    {
        public string ReturBeliId { get; set; }
        public string ReturBeliDate { get; set; }
        public string ReturBeliCode { get; set; }
        public string SupplierId { get; set; }
        public string WarehouseId { get; set; }
        public int TermOfPayment { get; set; }
        public string DueDate { get; set; }
        public string UserId { get; set; }
        public IEnumerable<SaveReturBeliRequestItem> ListBrg { get; set; }
    }

    public class SaveReturBeliRequestItem : IBrgKey
    {
        public string BrgId { get; set; }
        public string HrgInputStr { get; set; }
        public string QtyString { get; set; }
        public string DiscountString { get; set; }
        public decimal DppProsen { get; set; }
        public decimal PpnProsen { get; set; }
    }

    public interface ISaveReturBeliWorker : INunaService<ReturBeliModel, SaveReturBeliRequest> { }

    public class SaveReturBeliWorker : ISaveReturBeliWorker
    {
        private readonly IReturBeliBuilder _returBeliBuilder;
        private readonly IReturBeliWriter _returBeliWriter;
        private readonly IMediator _mediator;
        private readonly IGenStokReturBeliWorker _genStokWorker;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IBrgWriter _brgWriter;

        public SaveReturBeliWorker(IReturBeliBuilder returBeliBuilder,
            IReturBeliWriter returBeliWriter,
            IMediator mediator,
            IGenStokReturBeliWorker genStokWorker,
            IBrgBuilder brgBuilder,
            IBrgWriter brgWriter)
        {
            _returBeliBuilder = returBeliBuilder;
            _returBeliWriter = returBeliWriter;
            _mediator = mediator;
            _genStokWorker = genStokWorker;
            _brgBuilder = brgBuilder;
            _brgWriter = brgWriter;
        }

        public ReturBeliModel Execute(SaveReturBeliRequest req)
        {
            //  GUARD
            Guard.Argument(() => req).NotNull()
                .Member(x => x.ReturBeliDate, y => y.ValidDate("yyyy-MM-dd"))
                .Member(x => x.SupplierId, y => y.NotEmpty())
                .Member(x => x.WarehouseId, y => y.NotEmpty())
                .Member(x => x.DueDate, y => y.ValidDate("yyyy-MM-dd"));

            //  PROSES FAKTUR
            ReturBeliModel result;
            var fallback = Policy<ReturBeliModel>
                .Handle<KeyNotFoundException>()
                .Fallback(null as ReturBeliModel);
            var existingReturBeli = fallback.Execute(() => _returBeliBuilder.Load(req).Build());
            using (var trans = TransHelper.NewScope())
            {
                result = SaveReturBeli(req);
                UpdateHpp(result);

                trans.Complete();
            }

            //  RESULT
            return result;

            //  INNER FUNCTION
            void UpdateHpp(ReturBeliModel returBeliUpdateHpp)
            {
                var updateHppResult = new List<BrgModel>();
                foreach (var item in returBeliUpdateHpp.ListItem)
                {
                    var brg = _brgBuilder.Load(item).Build();
                    brg.Hpp = item.HppSat;
                    updateHppResult.Add(brg);
                }
                updateHppResult.ForEach(x => _brgWriter.Save(ref x));
            }
        }

        private ReturBeliModel SaveReturBeli(SaveReturBeliRequest req)
        {
            //  BUILD
            ReturBeliModel result;
            if (req.ReturBeliId.Length == 0)
            {
                result = _returBeliBuilder.CreateNew(req).Build();
            }
            else
            {
                result = _returBeliBuilder.Load(req).Build();
                result.ListItem.Clear();
            }

            result = _returBeliBuilder
                .Attach(result)
                .ReturBeliDate(req.ReturBeliDate.ToDate(DateFormatEnum.YMD))
                .ReturBeliCode(req.ReturBeliCode)
                .Supplier(req)
                .Warehouse(req)
                .IsPosted(false)
                .User(req)
                .TermOfPayment((TermOfPaymentEnum)req.TermOfPayment)
                .DueDate(req.DueDate.ToDate(DateFormatEnum.YMD))
                .Build();

            foreach (var item in req.ListBrg)
            {
                result = _returBeliBuilder
                    .Attach(result)
                    .AddItem(item, item.HrgInputStr, item.QtyString, item.DiscountString, item.DppProsen, item.PpnProsen)
                    .Build();
            }
            result = _returBeliBuilder
                .Attach(result)
                .CalcTotal()
                .Build();

            //  APPLY
            _ = _returBeliWriter.Save(result);
            _mediator.Publish(new SavedReturBeliEvent(req, result));
            return result;
        }
    }

    public class SavedReturBeliEvent : INotification
    {
        public SavedReturBeliEvent(SaveReturBeliRequest request, ReturBeliModel aggregate)
        {
            Request = request;
            Aggregate = aggregate;
        }
        public SaveReturBeliRequest Request { get; }
        public ReturBeliModel Aggregate { get; }
    }
}
