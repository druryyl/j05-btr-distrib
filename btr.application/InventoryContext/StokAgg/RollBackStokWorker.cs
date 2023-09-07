using btr.domain.InventoryContext.StokAgg;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.StokAgg.UseCases
{
    public interface IRollBackStokWorker : INunaService<bool, IReffKey>
    {
    }

    public class RollBackStokWorker : IRollBackStokWorker
    {
        private readonly IStokMutasiDal _stokMutasiDal;
        private readonly IStokBuilder _stokBuilder;
        private readonly IStokWriter _stokWriter;
        private readonly IRemoveStokWorker _removeStokWorker;

        public RollBackStokWorker(IStokMutasiDal stokMutasiDal,
            IStokBuilder stokBuilder,
            IStokWriter stokWriter,
            IRemoveStokWorker removeStokWorker)
        {
            _stokMutasiDal = stokMutasiDal;
            _stokBuilder = stokBuilder;
            _stokWriter = stokWriter;
            _removeStokWorker = removeStokWorker;
        }

        public bool Execute(IReffKey reffKey)
        {
            if (reffKey is null)
                return true;
            if (reffKey.ReffId == string.Empty)
                return true;

            var listStokMutasi = _stokMutasiDal.ListData(reffKey)?.ToList();
            if (listStokMutasi is null)
                return true;
            listStokMutasi.RemoveAll(x => x.JenisMutasi == "ROLLBACK");
            foreach(var item in listStokMutasi)
            {
                var stok = _stokBuilder.Load(item).Build();
                //  jika stok keluar, maka masukan
                if (item.QtyOut > 0)
                    AddStok(stok, item);
                else
                    RemoveStok(stok, item);
            }
            return true;
        }

        private void AddStok(StokModel stok, StokMutasiModel item)
        {
            var addedStok = _stokBuilder
                .Create(stok, stok, item.QtyOut, stok.NilaiPersediaan, item.ReffId, "ROLLBACK")
                .Build();
            _stokWriter.Save(ref addedStok);
        }

        private void RemoveStok(StokModel stok, StokMutasiModel item)
        {
            
            //  pertama remove sisa stok dirinya dulu
            var removedStok = _stokBuilder
                .Load(stok)
                .RemoveStok(stok.Qty, stok.NilaiPersediaan, item.ReffId, "ROLLBACK")
                .Build();
            _stokWriter.Save(ref removedStok);

            var qtyToBeRemovedInOtherStok = item.QtyIn - stok.Qty;
            var removeStokWorker = new RemoveStokRequest(stok.BrgId, stok.WarehouseId,
                qtyToBeRemovedInOtherStok, "", stok.NilaiPersediaan,item.ReffId, "ROLLBACK");
            _ = _removeStokWorker.Execute(removeStokWorker);
        }

    }
}
