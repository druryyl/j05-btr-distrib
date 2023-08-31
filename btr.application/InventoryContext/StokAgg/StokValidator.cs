using btr.domain.InventoryContext.StokAgg;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
