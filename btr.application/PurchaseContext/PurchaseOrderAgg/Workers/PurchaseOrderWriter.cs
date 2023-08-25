using btr.application.PurchaseContext.PurchaseOrderAgg.Contracts;
using btr.domain.PurchaseContext.PurchaseOrderAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using FluentValidation;

namespace btr.application.PurchaseContext.PurchaseOrderAgg.Workers
{
    public interface IPurchaseOrderWriter : INunaWriter<PurchaseOrderModel>
    {
    }

    public class PurchaseOrderWriter : IPurchaseOrderWriter
    {
        private readonly IValidator<PurchaseOrderModel> _validator;
        private readonly IPurchaseOrderDal _purchaseOrderDal;
        private readonly IPurchaseOrderItemDal _purchaseOrderItemDal;

        private readonly INunaCounterBL _counter;

        public PurchaseOrderWriter(IValidator<PurchaseOrderModel> validator, 
            IPurchaseOrderDal purchaseOrderDal, 
            IPurchaseOrderItemDal purchaseOrderItemDal, 
            INunaCounterBL counter)
        {
            _validator = validator;
            _purchaseOrderDal = purchaseOrderDal;
            _purchaseOrderItemDal = purchaseOrderItemDal;
            _counter = counter;
        }

        public void Save(ref PurchaseOrderModel model)
        {
            //  VALIDATE
            _validator.ValidateAndThrow(model);

            //  GEN-ID
            if (model.PurchaseOrderId.IsNullOrEmpty())
                model.PurchaseOrderId = _counter.Generate("PORD", IDFormatEnum.PREFYYMnnnnnC);
            var id = model.PurchaseOrderId;
            model.ListItem.ForEach(x => x.PurchaseOrderId = id);
            model.ListItem.ForEach(x => x.PurchaseOrderItemId = $"{id}-{x.NoUrut:D2}");

            //  WRITE
            using (var trans = TransHelper.NewScope())
            {
                var db = _purchaseOrderDal.GetData(model);
                if (db is null)
                    _purchaseOrderDal.Insert(model);
                else
                    _purchaseOrderDal.Update(model);

                _purchaseOrderItemDal.Delete(model);
                _purchaseOrderItemDal.Insert(model.ListItem);
                trans.Complete();
            }
        }
    }
}