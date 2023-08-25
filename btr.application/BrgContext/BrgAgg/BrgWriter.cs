using btr.domain.BrgContext.BrgAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using FluentValidation;

namespace btr.application.BrgContext.BrgAgg
{
    public interface IBrgWriter : INunaWriter<BrgModel>
    {
    }
    public class BrgWriter : IBrgWriter
    {
        private readonly IValidator<BrgModel> _validator;
        private readonly INunaCounterBL _counter;
        private readonly IBrgDal _brgDal;
        private readonly IBrgHargaDal _brgHargaDal;
        private readonly IBrgSatuanDal _brgSatuanDal;

        public BrgWriter(IValidator<BrgModel> validator, 
            INunaCounterBL counter, 
            IBrgDal brgDal, 
            IBrgHargaDal brgHargaDal, 
            IBrgSatuanDal brgSatuanDal)
        {
            _validator = validator;
            _counter = counter;
            _brgDal = brgDal;
            _brgHargaDal = brgHargaDal;
            _brgSatuanDal = brgSatuanDal;
        }

        public void Save(ref BrgModel model)
        {
            _validator.ValidateAndThrow(model);
            if (model.BrgId.IsNullOrEmpty())
                model.BrgId = _counter.Generate("BR", IDFormatEnum.PFnnnn);
            var id = model.BrgId;
            model.ListSatuan.ForEach(x => x.BrgId = id);
            model.ListHarga.ForEach(x => x.BrgId = id);

            using (var trans = TransHelper.NewScope())
            {
                var db = _brgDal.GetData(model);
                if (db is null)
                    _brgDal.Insert(model);
                else
                    _brgDal.Update(model);

                _brgSatuanDal.Delete(model);
                _brgHargaDal.Delete(model);
                _brgSatuanDal.Insert(model.ListSatuan);
                _brgHargaDal.Insert(model.ListHarga);
                
                trans.Complete();
            }
        }
    }
}