using btr.application.BrgContext.BrgAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.nuna.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.StokAgg.GenStokUseCase
{
    public class RemoveStokEditInvoiceRequest
    {
        public RemoveStokEditInvoiceRequest(string invoiceId)
        {
            InvoiceId = invoiceId;
        }
        public string InvoiceId { get; set; }
    }

    public interface IRemoveStokEditInvoiceWorker : INunaServiceVoid<RemoveStokEditInvoiceRequest>
    {
    }

    public class RemoveStokEditInvoiceWorker : IRemoveStokEditInvoiceWorker
    {
        private readonly IStokDal _stokDal;
        private readonly IStokMutasiDal _stokMutasiDal;
        private readonly IRemovePriorityStokWorker _removePriorityStokWorker;
        private readonly IBrgBuilder _brgBuilder;
        private readonly IStokBuilder _stokBuilder;
        private readonly IStokWriter _stokWriter;
        private readonly IGenStokBalanceWorker _stokBalanceWorker;

        public RemoveStokEditInvoiceWorker(IStokDal stokDal,
            IStokMutasiDal stokMutasiDal,
            IRemovePriorityStokWorker removePriorityStokWorker,
            IBrgBuilder brgBuilder,
            IStokBuilder stokBuilder,
            IStokWriter stokWriter,
            IGenStokBalanceWorker stokBalanceWorker)
        {
            _stokDal = stokDal;
            _stokMutasiDal = stokMutasiDal;
            _removePriorityStokWorker = removePriorityStokWorker;
            _brgBuilder = brgBuilder;
            _stokBuilder = stokBuilder;
            _stokWriter = stokWriter;
            _stokBalanceWorker = stokBalanceWorker;
        }

        public void Execute(RemoveStokEditInvoiceRequest req)
        {
            var listStokAll = _stokDal.ListData(new StokModel { ReffId = req.InvoiceId });
            if (listStokAll == null)
                return;
            var listStok= ListOnlyLastInvoice(listStokAll);
            var listMovingStok = new List<StokModel>();
            foreach (var item in listStok)
            {
                //  jika belum ada barang keluar, maka hapus
                if (item.Qty == item.QtyIn)
                {
                    _stokDal.Delete(item);
                    _stokMutasiDal.Delete(item);
                    continue;
                }

                var brg = _brgBuilder.Load(new BrgModel(item.BrgId)).Build();
                var stok = _stokBuilder
                    .Load(item)
                    .RemoveLastIn(req.InvoiceId)
                    .Build();
                listMovingStok.Add(stok);
            }

            //  WRITE
            using (var trans = TransHelper.NewScope())
            {
                //  stok
                foreach (var item in listMovingStok)
                {
                    _stokWriter.Save(item);
                    var stokBalanceReq = new GenStokBalanceRequest(item.BrgId, item.WarehouseId);
                    _stokBalanceWorker.Execute(stokBalanceReq);
                }
                trans.Complete();
            }
        }

        private IEnumerable<StokModel> ListOnlyLastInvoice(IEnumerable<StokModel> listStok)
        {
            var result = listStok
                .GroupBy(item => item.ReffId)
                .Select(group => group.Last())
                .ToList();
            return result;
        }
    }
}
