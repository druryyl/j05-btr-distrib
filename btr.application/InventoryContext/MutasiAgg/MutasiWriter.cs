using btr.domain.InventoryContext.MutasiAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using FluentValidation;

namespace btr.application.InventoryContext.MutasiAgg
{
    public interface IMutasiWriter : INunaWriter2<MutasiModel>
    {
    }
    public class MutasiWriter : IMutasiWriter
    {
        private readonly IMutasiDal _mutasiDal;
        private readonly IMutasiItemDal _mutasiItemDal;
        private readonly INunaCounterBL _counter;
        private readonly IValidator<MutasiModel> _validator;

        public MutasiWriter(IMutasiDal mutasiDal,
            IMutasiItemDal mutasiItemDal,
            INunaCounterBL counter,
            IValidator<MutasiModel> validator)
        {
            _mutasiDal = mutasiDal;
            _mutasiItemDal = mutasiItemDal;
            _counter = counter;
            _validator = validator;
        }

        public MutasiModel Save(MutasiModel model)
        {
            //  VALIDATE
            _validator.ValidateAndThrow(model);

            //  GENERATE-ID
            if (model.MutasiId.IsNullOrEmpty())
                model.MutasiId = _counter.Generate("MUTS", IDFormatEnum.PREFYYMnnnnnC);

            foreach (var item in model.ListItem)
            {
                item.MutasiId = model.MutasiId;
                item.MutasiItemId = $"{model.MutasiId}-{item.NoUrut:D2}";
            }

            //  WRITE
            using (var trans = TransHelper.NewScope())
            {
                var db = _mutasiDal.GetData(model);
                if (db is null)
                    _mutasiDal.Insert(model);
                else
                    _mutasiDal.Update(model);

                _mutasiItemDal.Delete(model);

                _mutasiItemDal.Insert(model.ListItem);

                trans.Complete();
            }
            return model;
        }
    }
}