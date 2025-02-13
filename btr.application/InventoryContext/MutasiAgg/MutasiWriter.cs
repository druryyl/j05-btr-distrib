using btr.domain.InventoryContext.MutasiAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using FluentValidation;
using System.Linq;

namespace btr.application.InventoryContext.MutasiAgg
{
    public interface IMutasiWriter : INunaWriter2<MutasiModel>
    {
    }
    public class MutasiWriter : IMutasiWriter
    {
        private readonly IMutasiDal _mutasiDal;
        private readonly IMutasiItemDal _mutasiItemDal;
        private readonly IMutasiDiscDal _mutasiDiscDal;
        private readonly INunaCounterBL _counter;
        private readonly IValidator<MutasiModel> _validator;

        public MutasiWriter(IMutasiDal mutasiDal,
            IMutasiItemDal mutasiItemDal,
            INunaCounterBL counter,
            IValidator<MutasiModel> validator,
            IMutasiDiscDal mutasiDiscDal)
        {
            _mutasiDal = mutasiDal;
            _mutasiItemDal = mutasiItemDal;
            _counter = counter;
            _validator = validator;
            _mutasiDiscDal = mutasiDiscDal;
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
                foreach(var item2 in item.ListDisc)
                {
                    item2.MutasiId = model.MutasiId;
                    item2.MutasiItemId = item.MutasiItemId;
                    item2.MutasiDiscId = $"{item2.MutasiItemId}-{item2.NoUrut:D2}";
                }
            }

            var listAllDisc = model.ListItem.SelectMany(x => x.ListDisc);

            //  WRITE
            using (var trans = TransHelper.NewScope())
            {
                var db = _mutasiDal.GetData(model);
                if (db is null)
                    _mutasiDal.Insert(model);
                else
                    _mutasiDal.Update(model);

                _mutasiItemDal.Delete(model);
                _mutasiDiscDal.Delete(model);

                _mutasiItemDal.Insert(model.ListItem);
                _mutasiDiscDal.Insert(listAllDisc);

                trans.Complete();
            }
            return model;
        }
    }
}