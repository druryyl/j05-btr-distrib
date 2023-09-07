using btr.domain.InventoryContext.StokAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using FluentValidation;

namespace btr.application.InventoryContext.StokAgg
{
    public interface IStokWriter : INunaWriter2<StokModel>
    {
    }
    
    public class StokWriter : IStokWriter
    {
        private readonly IStokDal _stokDal;
        private readonly IStokMutasiDal _stokMutasiDal;
        private readonly INunaCounterBL _counter;
        private readonly IValidator<StokModel> _validator;

        public StokWriter(IStokDal stokDal,
            IStokMutasiDal stokMutasiDal,
            INunaCounterBL counter,
            IValidator<StokModel> validator)
        {
            _stokDal = stokDal;
            _stokMutasiDal = stokMutasiDal;
            _counter = counter;
            _validator = validator;
        }

        public StokModel Save(StokModel model)
        {
            _validator.ValidateAndThrow(model);
            if (model.StokId.IsNullOrEmpty())
                model.StokId = _counter.Generate("STOK", IDFormatEnum.PREFYYMnnnnnC);

            var id = model.StokId;
            model.ListMutasi.ForEach(x => x.StokId = id);
            model.ListMutasi.ForEach(x => x.StokMutasiId = $"{id}-{x.NoUrut.ToString().PadLeft(2, '0')}");

            var db = _stokDal.GetData(model);

            using (var trans = TransHelper.NewScope())
            {
                if (db is null)
                    _stokDal.Insert(model);
                else
                    _stokDal.Update(model);

                _stokMutasiDal.Delete(model);
                _stokMutasiDal.Insert(model.ListMutasi);

                trans.Complete();
            }
            return model;
        }
    }
}