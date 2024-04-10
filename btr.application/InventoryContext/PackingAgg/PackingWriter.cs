using btr.domain.InventoryContext.PackingAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

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
        private readonly IPackingBrgDal _packingFakturSupplierDal;

        public PackingWriter(INunaCounterBL counter, 
            IPackingDal packingDal, 
            IPackingFakturDal packingFakturDal, 
            IPackingBrgDal packingFakturSupplierDal)
        {
            _counter = counter;
            _packingDal = packingDal;
            _packingFakturDal = packingFakturDal;
            _packingFakturSupplierDal = packingFakturSupplierDal;
        }

        public PackingModel Save(PackingModel model)
        {
            if (model.PackingId.IsNullOrEmpty())
                model.PackingId = _counter.Generate("PACK", IDFormatEnum.PREFYYMnnnnnC);

            model.ListFaktur.ForEach(x => x.PackingId = model.PackingId);
            model.ListBrg.ForEach(x => x.PackingId = model.PackingId);

            var db = _packingDal.GetData(model);
            using (var trans = TransHelper.NewScope())
            {
                if (db is null)
                    _packingDal.Insert(model);
                else
                    _packingDal.Update(model);

                _packingFakturDal.Delete(model);
                _packingFakturSupplierDal.Delete(model);
                _packingFakturDal.Insert(model.ListFaktur);
                _packingFakturSupplierDal.Insert(model.ListBrg);
                trans.Complete();
            }

            return model;

        }
    }
}
