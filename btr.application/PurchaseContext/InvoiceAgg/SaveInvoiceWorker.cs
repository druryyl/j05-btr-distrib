using System.Collections.Generic;
using System.Linq;
using btr.application.InventoryContext.StokAgg.GenStokUseCase;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.PurchaseContext.InvoiceAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using Dawn;
using MediatR;
using Polly;

namespace btr.application.PurchaseContext.InvoiceAgg
{
    public class SaveInvoiceRequest : IInvoiceKey, ISupplierKey, 
        IWarehouseKey, IUserKey
    {
        public string InvoiceId { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceCode { get; set; }
        public string SupplierId { get; set; }
        public string WarehouseId { get; set; }
        public int TermOfPayment { get; set; }
        public string DueDate { get; set; }
        public string UserId { get; set; }
        public IEnumerable<SaveInvoiceRequestItem> ListBrg { get; set; }
    }

    public class SaveInvoiceRequestItem : IBrgKey
    {
        public string BrgId { get; set; }
        public string HrgInputStr { get; set; }
        public string QtyString { get; set; }
        public string DiscountString { get; set; }
        public decimal PpnProsen { get; set; }
    }

    public interface ISaveInvoiceWorker : INunaService<InvoiceModel, SaveInvoiceRequest> { }

    public class SaveInvoiceWorker : ISaveInvoiceWorker
    {
        private readonly IInvoiceBuilder _invoiceBuilder;
        private readonly IInvoiceWriter _invoiceWriter;
        private readonly IMediator _mediator;
        private readonly IGenStokInvoiceWorker _genStokWorker;

        public SaveInvoiceWorker(IInvoiceBuilder invoiceBuilder,
            IInvoiceWriter invoiceWriter,
            IMediator mediator, IGenStokInvoiceWorker genStokWorker)
        {
            _invoiceBuilder = invoiceBuilder;
            _invoiceWriter = invoiceWriter;
            _mediator = mediator;
            _genStokWorker = genStokWorker;
        }

        public InvoiceModel Execute(SaveInvoiceRequest req)
        {
            //  GUARD
            Guard.Argument(() => req).NotNull()
                .Member(x => x.InvoiceDate, y => y.ValidDate("yyyy-MM-dd"))
                .Member(x => x.SupplierId, y => y.NotEmpty())
                .Member(x => x.WarehouseId, y => y.NotEmpty())
                .Member(x => x.DueDate, y => y.ValidDate("yyyy-MM-dd"));

            //  PROSES FAKTUR
            InvoiceModel result;
            var fallback = Policy<InvoiceModel>
                .Handle<KeyNotFoundException>()
                .Fallback(null as InvoiceModel);
            var existingInvoice = fallback.Execute(() => _invoiceBuilder.Load(req).Build());
                
            using (var trans = TransHelper.NewScope())
            {
                result = SaveInvoice(req);

                if (IsStokChanged(existingInvoice, result))
                {
                    var genStokReq = new GenStokInvoiceRequest(result.InvoiceId);
                    _genStokWorker.Execute(genStokReq);
                }

                trans.Complete();
            }
            
            //  RESULT
            return result;
            
            bool IsStokChanged(InvoiceModel invoiceDb, InvoiceModel invoiceInput)
            {
                if (invoiceDb is null) return true;
                if (invoiceDb.ListItem.Count != invoiceInput.ListItem.Count) return true;
                var invDbList = invoiceDb.ListItem.Select(x => new {x.BrgId, x.QtyPotStok, x.HppSat}).ToList();
                var invInputList = invoiceInput.ListItem.Select(x => new {x.BrgId, x.QtyPotStok, x.HppSat}).ToList();

                //  compare invDbList vs invInputList
                //  if any item is different, then return true
                return invDbList
                    .Any(x => !invInputList
                        .Any(y => y.BrgId == x.BrgId && y.QtyPotStok == x.QtyPotStok && y.HppSat == x.HppSat));
            }             
        }

        private InvoiceModel SaveInvoice(SaveInvoiceRequest req)
        {
            //  BUILD
            InvoiceModel result;
            if (req.InvoiceId.Length == 0)
            {
                result = _invoiceBuilder.CreateNew(req).Build();
            }
            else
            {
                result = _invoiceBuilder.Load(req).Build();
                result.ListItem.Clear();
            }

            result = _invoiceBuilder
                .Attach(result)
                .InvoiceDate(req.InvoiceDate.ToDate(DateFormatEnum.YMD))
                .InvoiceCode(req.InvoiceCode)
                .Supplier(req)
                .Warehouse(req)
                .User(req)
                .TermOfPayment((TermOfPaymentEnum)req.TermOfPayment)
                .DueDate(req.DueDate.ToDate(DateFormatEnum.YMD))
                .Build();

            foreach (var item in req.ListBrg)
            {
                result = _invoiceBuilder
                    .Attach(result)
                    .AddItem(item, item.HrgInputStr, item.QtyString, item.DiscountString, item.PpnProsen)
                    .Build();
            }
            result = _invoiceBuilder
                .Attach(result)
                .CalcTotal()
                .Build();

            //  APPLY
            _ = _invoiceWriter.Save(result);
            _mediator.Publish(new SavedInvoiceEvent(req, result));
            return result;
        }
    }

    public class SavedInvoiceEvent : INotification
    {
        public SavedInvoiceEvent(SaveInvoiceRequest request, InvoiceModel aggregate)
        {
            Request = request;
            Aggregate = aggregate;
        }
        public SaveInvoiceRequest Request { get; }
        public InvoiceModel Aggregate { get; }
    }
}
