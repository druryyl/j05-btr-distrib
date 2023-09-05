using btr.domain.InventoryContext.OpnameAgg;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.OpnameAgg
{
    public class OpnameValidator : AbstractValidator<OpnameModel>
    {
        public OpnameValidator()
        {
        }
    }
}
