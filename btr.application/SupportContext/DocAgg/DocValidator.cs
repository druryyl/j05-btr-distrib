using btr.domain.SupportContext.DocAgg;
using FluentValidation;

namespace btr.application.SupportContext.DocAgg
{
    public class DocValidator : AbstractValidator<DocModel>
    {
        public DocValidator()
        {
            RuleFor(x => x.DocId).NotEmpty();
            RuleFor(x => x.WarehouseId).NotEmpty();
        }
    }
}
