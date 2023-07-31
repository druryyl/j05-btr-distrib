using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using FluentValidation;

namespace btr.application.SalesContext.SalesPersonAgg.Workers
{
    public interface ISalesPersonWriter : INunaWriter<SalesPersonModel>
    {
    }

    public class SalesPersonWriter : ISalesPersonWriter
    {
        private readonly ISalesPersonDal _salesPersonDal;
        private readonly IValidator<SalesPersonModel> _validator;
        private readonly INunaCounterBL _counter;

        public SalesPersonWriter(ISalesPersonDal salesPersonDal,
            IValidator<SalesPersonModel> validator,
            INunaCounterBL counter)
        {
            _salesPersonDal = salesPersonDal;
            _validator = validator;
            _counter = counter;
        }

        public void Save(ref SalesPersonModel model)
        {
            //  VALIDATE
            _validator.ValidateAndThrow(model);

            //  GEN-ID
            if (model.SalesPersonId.IsNullOrEmpty())
                model.SalesPersonId = _counter.Generate("SP", IDFormatEnum.PFnnn);

            //  WRITE
            var persisted = _salesPersonDal.GetData(model);
            if (persisted is null)
                _salesPersonDal.Insert(model);
            else
                _salesPersonDal.Update(model);
        }
    }
}