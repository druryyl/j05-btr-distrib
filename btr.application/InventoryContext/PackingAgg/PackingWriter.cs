using btr.domain.InventoryContext.PackingAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.PackingAgg
{
    public interface IPackingWriter : INunaWriter2<PackingModel>
    {
    }

    public class PackingWriter : IPackingWriter
    {
        private readonly INunaCounterBL _counter;
        private readonly IPackingDal _packingDal;
        private readonly IPackingFakturDal _packingFakturDal;
        private readonly IPackingBrgDal _packingSupplierDal;

        public PackingWriter(INunaCounterBL counter, 
            IPackingDal packingDal, 
            IPackingFakturDal packingFakturDal, 
            IPackingBrgDal packingSupplierDal)
        {
            _counter = counter;
            _packingDal = packingDal;
            _packingFakturDal = packingFakturDal;
            _packingSupplierDal = packingSupplierDal;
        }

        public PackingModel Save(PackingModel model)
        {
            if (model.PackingId.IsNullOrEmpty())
                model.PackingId = _counter.Generate("PACK", IDFormatEnum.PREFYYMnnnnnC);

            model.ListFaktur.ForEach(x => x.PackingId = model.PackingId);
            model.ListSupplier.ForEach(x => x.PackingId = model.PackingId);
            var listAllBrg = model.ListSupplier
                .SelectMany(hdr => hdr.ListBrg, (hdr, dtl) => dtl)?.ToList()??new List<PackingBrgModel>();
            var db = _packingDal.GetData(model);
            using (var trans = TransHelper.NewScope())
            {
                if (db is null)
                    _packingDal.Insert(model);
                else
                    _packingDal.Update(model);

                _packingFakturDal.Delete(model);
                _packingSupplierDal.Delete(model);
                _packingFakturDal.Insert(model.ListFaktur);
                _packingSupplierDal.Insert(listAllBrg);
                trans.Complete();
            }

            return model;

        }
    }
}
