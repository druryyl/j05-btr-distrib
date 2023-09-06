using btr.application.InventoryContext.StokAgg;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.FakturAgg.Workers
{
    public class SaveFakturCommand : IFakturKey, ICustomerKey, ISalesPersonKey, IWarehouseKey, IUserKey
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
        public IEnumerable<SaveFakturCommandItem> ListBrg { get; set; }
    }

    public class SaveFakturCommandItem : IBrgKey
    {
        public string BrgId { get; set; }
        public string QtyString { get; set; }
        public string DiscountString { get; set; }
        public decimal PpnProsen { get; set; }
    }

    public class SaveFakturWorker : INunaService<FakturModel, SaveFakturCommand>
    {
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IFakturWriter _fakturWriter;
        private readonly IAddStokWorker _addStokWorker;

        public SaveFakturWorker(IFakturBuilder fakturBuilder,
            IFakturWriter fakturWriter,
            IAddStokWorker addStokWorker)
        {
            _fakturBuilder = fakturBuilder;
            _fakturWriter = fakturWriter;
            _addStokWorker = addStokWorker;
        }

        public FakturModel Execute(SaveFakturCommand req)
        {
            //  GUARD
            Guard.Argument(() => req).NotNull()
                .Member(x => x.FakturDate, y => y.ValidDate("yyyy-MM-dd"))
                .Member(x => x.CustomerId, y => y.NotEmpty())
                .Member(x => x.SalesPersonId, y => y.NotEmpty())
                .Member(x => x.WarehouseId, y => y.NotEmpty())
                .Member(x => x.DueDate, y => y.ValidDate("yyyy-MM-dd"));

            //  PROSES FAKTUR
            var faktur = SaveFaktur(req, out FakturModel prevState);
            GenStok(faktur, prevState);

            //  RESULT
            return faktur;
        }
        private FakturModel SaveFaktur(SaveFakturCommand req, out FakturModel prevState)
        {
            //  BUILD
            FakturModel result;
            if (req.FakturId.Length == 0)
            {
                result = _fakturBuilder.CreateNew(req).Build();
                prevState = result;
            }
            else
            {
                result = _fakturBuilder.Load(req).Build();
                prevState = null;
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
            return result;
        }
        private void GenStok(FakturModel faktur, FakturModel prevState)
        {
            if (prevState != null)
                Rollback(prevState);
            //  TODO: Create Remove Stok Request
            foreach (var item in prevState.ListItem)
            {
                var sat = item.ListQtyHarga.FirstOrDefault(x => x.Conversion == 1);
                var qty = item.ListQtyHarga.Sum(x => x.Qty * x.Conversion);
                var removeStok = new RemoveStokRequest(item.BrgId, prev.WarehouseId,
                    qty, sat.Satuan, item.HargaJual, prev.FakturId, "FAKTUR");
                _addStokWorker.Execute(addStok);
            }

        }

        private void Rollback(FakturModel prev)
        {
            foreach(var item in prev.ListItem)
            {
                var sat = item.ListQtyHarga.FirstOrDefault(x => x.Conversion == 1);
                var qty = item.ListQtyHarga.Sum(x => x.Qty * x.Conversion);
                var addStok = new AddStokRequest(item.BrgId, prev.WarehouseId,
                    qty, sat.Satuan, item.HargaJual, prev.FakturId, "FAKTUR-ROLLBACK");
                _addStokWorker.Execute(addStok);
            }
        }
    }
}
