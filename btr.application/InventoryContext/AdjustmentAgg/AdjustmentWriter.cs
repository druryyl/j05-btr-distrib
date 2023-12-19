using btr.domain.InventoryContext.AdjustmentAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.InventoryContext.AdjustmentAgg
{
    public interface IAdjustmentWriter : INunaWriter2<AdjustmentModel>
    {
        
        
        
        
        
        
    }
    public class AdjustmentWriter : IAdjustmentWriter
    {
        private readonly IAdjustmentDal _adjustmentDal;
        private readonly INunaCounterBL _counter;

        public AdjustmentWriter(IAdjustmentDal adjustmentDal, INunaCounterBL counter)
        {
            _adjustmentDal = adjustmentDal;
            _counter = counter;
        }

        public AdjustmentModel Save(AdjustmentModel model)
        {
            if (model.AdjustmentId.IsNullOrEmpty())
                model.AdjustmentId = _counter.Generate("ADJS", IDFormatEnum.PREFYYMnnnnnC);

            var db = _adjustmentDal.GetData(model);
            if (db is null)
                _adjustmentDal.Insert(model);
            else
                _adjustmentDal.Update(model);
            
            return model;
        }
    }
}