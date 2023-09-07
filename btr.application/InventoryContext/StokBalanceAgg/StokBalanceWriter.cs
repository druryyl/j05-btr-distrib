using btr.domain.InventoryContext.StokBalanceAgg;
using btr.nuna.Application;

namespace btr.application.InventoryContext.StokBalanceAgg
{
    public interface IStokBalanceWriter : INunaWriter<StokBalanceModel>
    {
    }

    public class StokBalanceWriter : IStokBalanceWriter
    {
        private readonly IStokBalanceWarehouseDal _stokBalanceDal;

        public StokBalanceWriter(IStokBalanceWarehouseDal stokBalanceDal)
        {
            _stokBalanceDal = stokBalanceDal;
        }

        public void Save(ref StokBalanceModel model)
        {
            foreach(var item in model.ListWarehouse)
                item.BrgId = model.BrgId;

            using(var trans = TransHelper.NewScope())
            {
                _stokBalanceDal.Delete(model);
                _stokBalanceDal.Insert(model.ListWarehouse);
                trans.Complete();
            }
        }
    }
}
