using btr.domain.SalesContext.FakturControlAgg;
using btr.nuna.Application;

namespace btr.application.SalesContext.FakturControlAgg
{
    public interface IFakturControlWriter : INunaWriter2<FakturControlModel>
    {
    }
    public class FakturControlWriter : IFakturControlWriter
    {
        private readonly IFakturControlStatusDal _fakturControlStatusDal;

        public FakturControlWriter(IFakturControlStatusDal fakturControlStatusDal)
        {
            _fakturControlStatusDal = fakturControlStatusDal;
        }

        public FakturControlModel Save(FakturControlModel model)
        {
            foreach (var item in model.ListStatus)
                item.FakturId = model.FakturId;
            
            using (var trans = TransHelper.NewScope())
            {
                _fakturControlStatusDal.Delete(model);
                _fakturControlStatusDal.Insert(model.ListStatus);
                trans.Complete();
            }
            return model;
        }
    }
}
