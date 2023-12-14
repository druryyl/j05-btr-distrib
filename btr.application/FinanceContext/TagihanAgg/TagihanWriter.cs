using btr.domain.FinanceContext.TagihanAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.FinanceContext.TagihanAgg
{
    public interface ITagihanWriter : INunaWriter2<TagihanModel>
    {
    }
    public class TagihanWriter : ITagihanWriter
    {
        private readonly ITagihanDal _tagihanDal;
        private readonly ITagihanFakturDal _tagihanFakturDal;
        private readonly INunaCounterBL _counter;

        public TagihanWriter(ITagihanDal tagihanDal, 
            ITagihanFakturDal tagihanFakturDal, 
            INunaCounterBL counter)
        {
            _tagihanDal = tagihanDal;
            _tagihanFakturDal = tagihanFakturDal;
            _counter = counter;
        }

        public TagihanModel Save(TagihanModel model)
        {
            if (model.TagihanId.IsNullOrEmpty())
                model.TagihanId = _counter.Generate("TAGH", IDFormatEnum.PREFYYMnnnnnC);
            model.ListFaktur.ForEach(x => x.TagihanId = model.TagihanId);
            
            var db = _tagihanDal.GetData(model);

            using (var trans = TransHelper.NewScope())
            {
                if (db is null)
                    _tagihanDal.Insert(model);
                else
                    _tagihanDal.Update(model);
                
                _tagihanFakturDal.Delete(model);
                _tagihanFakturDal.Insert(model.ListFaktur);
                
                trans.Complete();
            }

            return model;
        }
    }
}