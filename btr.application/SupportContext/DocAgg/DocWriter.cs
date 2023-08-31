using btr.domain.SupportContext.DocAgg;
using btr.nuna.Application;
using FluentValidation;

namespace btr.application.SupportContext.DocAgg
{
    public interface IDocWriter : INunaWriter<DocModel>
    {
    }

    public class DocWriter : IDocWriter
    {
        private readonly IValidator<DocModel> _validator;
        private readonly IDocDal _docDal;
        private readonly IDocActionDal _docActionDal;



        public DocWriter(IValidator<DocModel> validator, 
            IDocDal docDal, 
            IDocActionDal docActionDal)
        {
            _validator = validator;
            _docDal = docDal;
            _docActionDal = docActionDal;
        }

        public void Save(ref DocModel model)
        {
            _validator.ValidateAndThrow(model);
            var db = _docDal.GetData(model);
            var id = model.DocId;
            model.ListAction.ForEach(x => x.DocId = id);

            using (var trans = TransHelper.NewScope())
            { 
                if (db is null)
                    _docDal.Insert(model);
                else
                    _docDal.Update(model);

                _docActionDal.Delete(model);
                _docActionDal.Insert(model.ListAction);
                trans.Complete();
            }
        }
    }
}
