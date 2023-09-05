using btr.domain.InventoryContext.OpnameAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using FluentValidation;

namespace btr.application.InventoryContext.OpnameAgg
{
    public interface IOpnameWriter : INunaWriter<OpnameModel>
    {
    } 
    
    public class OpnameWriter : IOpnameWriter
    {
        private readonly IValidator<OpnameModel> _validator;
        private readonly INunaCounterBL _counter;
        private readonly IOpnameDal _opnameDal; 

        public OpnameWriter(IValidator<OpnameModel> validator, INunaCounterBL counter, IOpnameDal opnameDal)
        {
            _validator = validator;
            _counter = counter;
            _opnameDal = opnameDal;
        }

        public void Save(ref OpnameModel model)
        {
            _validator.ValidateAndThrow(model);
            if (model.OpnameId.IsNullOrEmpty())
                model.OpnameId = _counter.Generate("OPNM", IDFormatEnum.PREFYYMnnnnnC);

            var db = _opnameDal.GetData(model);
            if (db is null)
                _opnameDal.Insert(model);
            else
                _opnameDal.Update(model);
        }
    }
}