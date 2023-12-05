using btr.domain.InventoryContext.OpnameAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.InventoryContext.OpnameAgg
{
    public interface IStokOpWriter : INunaWriter2<StokOpModel>
    {
    }
    public class StokOpWriter : IStokOpWriter
    {
        private readonly IStokOpDal _stokOpDal;
        private readonly INunaCounterBL _counter;
        
        public StokOpWriter(IStokOpDal stokOpDal, INunaCounterBL counter)
        {
            _stokOpDal = stokOpDal;
            _counter = counter;
        }

        public StokOpModel Save(StokOpModel model)
        {
            if (model.StokOpId.IsNullOrEmpty())
                model.StokOpId = _counter.Generate("SOPN", IDFormatEnum.PREFYYMnnnnnC);
            var db = _stokOpDal.GetData(model);
            if (db is null)
                _stokOpDal.Insert(model);
            else
                _stokOpDal.Update(model);
            return model;
        }
    }
}