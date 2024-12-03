using btr.domain.FinanceContext.ReturBalanceAgg;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.FinanceContext.FakturPotBalanceAgg
{
    public interface IFakturPotBalanceWriter : INunaWriter2<FakturPotBalanceModel>
    {
    }

    public class FakturPotBalanceWriter : IFakturPotBalanceWriter
    {
        private readonly IFakturPotBalanceDal _fakturPotBalanceDal;
        private readonly IFakturPotBalancePostDal _fakturPotBalancePostDal;

        public FakturPotBalanceWriter(IFakturPotBalanceDal fakturPotBalanceDal, 
            IFakturPotBalancePostDal fakturPotBalancePostDal)
        {
            _fakturPotBalanceDal = fakturPotBalanceDal;
            _fakturPotBalancePostDal = fakturPotBalancePostDal;
        }


        public FakturPotBalanceModel Save(FakturPotBalanceModel model)
        {
            var existing = _fakturPotBalanceDal.GetData(model);

            using (var trans = TransHelper.NewScope())
            {
                if (existing is null)
                    _fakturPotBalanceDal.Insert(model);
                else
                    _fakturPotBalanceDal.Update(model);

                _fakturPotBalancePostDal.Delete(model);
                _fakturPotBalancePostDal.Insert(model.ListPost);

                trans.Complete();
            }

            return existing;
        }
    }
}
