using btr.domain.FinanceContext.ReturBalanceAgg;
using btr.nuna.Application;

namespace btr.application.FinanceContext.ReturBalanceAgg
{
    public interface IReturBalanceWriter : INunaWriter2<ReturBalanceModel>
    {
    }

    public class ReturBalanceWriter : IReturBalanceWriter
    {
        private readonly IReturBalanceDal _returBalanceDal;
        private readonly IReturBalancePostDal _returBalancePostDal;

        public ReturBalanceWriter(IReturBalanceDal returBalanceDal, 
            IReturBalancePostDal returBalancePostDal)
        {
            _returBalanceDal = returBalanceDal;
            _returBalancePostDal = returBalancePostDal;
        }

        public ReturBalanceModel Save(ReturBalanceModel model)
        {
            var existing = _returBalanceDal.GetData(model);

            using (var trans = TransHelper.NewScope())
            {
                if (existing is null)
                    _returBalanceDal.Insert(model);
                else
                    _returBalanceDal.Update(model);

                _returBalancePostDal.Delete(model);
                _returBalancePostDal.Insert(model.ListPost);

                trans.Complete();
            }

            return model;
        }
    }
}
