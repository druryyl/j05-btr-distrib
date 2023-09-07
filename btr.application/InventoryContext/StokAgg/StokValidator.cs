using btr.domain.InventoryContext.StokAgg;
using FluentValidation;

namespace btr.application.InventoryContext.StokAgg
{
    public class StokValidator : AbstractValidator<StokModel>
    {
        public StokValidator()
        {
            RuleFor(x => x.BrgId).NotEmpty();
            RuleFor(x => x.WarehouseId).NotEmpty();
        }
    }
}
