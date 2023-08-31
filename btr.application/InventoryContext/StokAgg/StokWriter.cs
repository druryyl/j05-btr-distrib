using btr.domain.InventoryContext.StokAgg;
using btr.nuna.Application;

namespace btr.application.InventoryContext.StokAgg
{
    public interface IStokWriter : INunaWriter<StokModel>
    {
    }
    
    public class StokWriter : IStokWriter
    {
        private readonly IStokDal _stokDal;
        private readonly IStokMutasiDal _stokMutasiDal;
        private readonly INunaCounterBL _counter;

        public StokWriter(IStokDal stokDal, 
            IStokMutasiDal stokMutasiDal, 
            INunaCounterBL counter)
        {
            _stokDal = stokDal;
            _stokMutasiDal = stokMutasiDal;
            _counter = counter;
        }

        public void Save(ref StokModel model)
        {
            
        }
    }
}