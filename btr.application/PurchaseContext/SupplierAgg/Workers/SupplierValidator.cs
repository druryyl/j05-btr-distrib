using btr.domain.PurchaseContext.SupplierAgg;
using FluentValidation;

namespace btr.application.PurchaseContext.SupplierAgg.Workers
{
    public class SupplierValidator : AbstractValidator<SupplierModel>
    {
        public SupplierValidator()
        {
            RuleFor(x => x.SupplierName)
                .NotEmpty()
                .MaximumLength(50);
        }
        
    }
}