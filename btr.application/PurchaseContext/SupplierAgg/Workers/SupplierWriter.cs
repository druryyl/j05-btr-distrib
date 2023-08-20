using btr.domain.PurchaseContext.SupplierAgg;
using btr.nuna.Application;
using FluentValidation;

namespace btr.application.PurchaseContext.SupplierAgg.Workers
{
    public interface ISupplierWriter : INunaWriter<SupplierModel>
    {
    }

    public class SupplierWriter : ISupplierWriter
    {
        private readonly IValidator<SupplierModel> _validator;

        public SupplierWriter(IValidator<SupplierModel> validator)
        {
            _validator = validator;
        }

        public void Save(ref SupplierModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}