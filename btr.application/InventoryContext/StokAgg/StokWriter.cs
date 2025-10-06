using btr.domain.InventoryContext.StokAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using FluentValidation;
using System.Linq;

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
            model.ListMutasi.ForEach(x => x.StokMutasiId = $"{id}-{x.NoUrut.ToString().PadLeft(4, '0')}");

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

        public StokModel SaveDifferential(StokModel model)
        {
            _validator.ValidateAndThrow(model);
            if (model.StokId.IsNullOrEmpty())
                model.StokId = _counter.Generate("STOK", IDFormatEnum.PREFYYMnnnnnC);

            var id = model.StokId;
            model.ListMutasi.ForEach(x => x.StokId = id);
            model.ListMutasi.ForEach(x => x.StokMutasiId = $"{id}-{x.NoUrut.ToString().PadLeft(4, '0')}");

            var db = _stokDal.GetData(model);
            // --- Synchronize Mutasi ---
            var currentPersistedData = _stokMutasiDal.ListData((IStokKey)model);

            // Build lookup for efficiency
            var currentMap = currentPersistedData.ToDictionary(x => x.StokMutasiId, x => x);

            // 1️⃣ Identify new records (Insert)
            var newRecords = model.ListMutasi
                .Where(x => !currentMap.ContainsKey(x.StokMutasiId))
                .ToList();

            // 2️⃣ Identify updated records (Update)
            var updatedRecords = model.ListMutasi
                .Where(x =>
                    currentMap.TryGetValue(x.StokMutasiId, out var existing) &&
                    !AreMutasiEqual(x, existing))
                .ToList();

            // 3️⃣ Identify deleted records (Delete)
            var deletedRecords = currentPersistedData
                .Where(x => !model.ListMutasi.Any(y => y.StokMutasiId == x.StokMutasiId))
                .ToList();
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
        private bool AreMutasiEqual(StokMutasiModel a, StokMutasiModel b)
        {
            // Compare only meaningful business properties
            return a.StokId == b.StokId
                && a.NoUrut == b.NoUrut
                && a.BrgId == b.BrgId
                && a.QtyIn == b.QtyIn
                && a.QtyOut == b.QtyOut;
        }

    }
}