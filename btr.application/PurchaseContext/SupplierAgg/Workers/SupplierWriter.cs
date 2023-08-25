using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using FluentValidation;

namespace btr.application.PurchaseContext.SupplierAgg.Workers
{
    public interface ISupplierWriter : INunaWriter<SupplierModel>
    {
    }

    public class SupplierWriter : ISupplierWriter
    {
        private readonly IValidator<SupplierModel> _validator;
        private readonly INunaCounterBL _counter;
        private readonly ISupplierDal _supplierDal;

        public SupplierWriter(IValidator<SupplierModel> validator, 
            INunaCounterBL counter, 
            ISupplierDal supplierDal)
        {
            _validator = validator;
            _counter = counter;
            _supplierDal = supplierDal;
        }

        public void Save(ref SupplierModel model)
        {
            _validator.ValidateAndThrow(model);
            if (model.SupplierId.IsNullOrEmpty())
                model.SupplierId = _counter.Generate("SP", IDFormatEnum.PFnnn);

            var db = _supplierDal.GetData(model);
            if (db is null)
                _supplierDal.Insert(model);
            else
                _supplierDal.Update(model);
        }
    }
}