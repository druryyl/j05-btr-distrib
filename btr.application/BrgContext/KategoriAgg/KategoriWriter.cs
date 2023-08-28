using btr.domain.BrgContext.KategoriAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.BrgContext.KategoriAgg
{
    public interface IKategoriWriter : INunaWriter<KategoriModel>
    {

    }
    public class KategoriWriter : IKategoriWriter
    {
        private readonly IKategoriDal _kategoriDal;
        private readonly INunaCounterBL _counter;

        public KategoriWriter(IKategoriDal kategoriDal, 
            INunaCounterBL counter)
        {
            _kategoriDal = kategoriDal;
            _counter = counter;
        }

        public void Save(ref KategoriModel model)
        {
            if (model.KategoriId.IsNullOrEmpty())
                model.KategoriId = _counter.Generate("KT", IDFormatEnum.PFnnn);
            var db = _kategoriDal.GetData(model);
            if (db is null)
                _kategoriDal.Insert(model);
            else
                _kategoriDal.Update(model);
        }
    }
}
