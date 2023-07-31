using System.Threading;
using System.Threading.Tasks;
using btr.application.SalesContext.SalesPersonAgg.Workers;
using btr.domain.SalesContext.SalesPersonAgg;
using Dawn;
using MediatR;

namespace btr.application.SalesContext.SalesPersonAgg.UseCases
{
    public class UpdateSalesPersonCommand : IRequest, ISalesPersonKey
    {
        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
    }


    public class UpdateSalesPersonHandler : IRequestHandler<UpdateSalesPersonCommand>
    {
        private SalesPersonModel _aggRoot = new SalesPersonModel();
        private readonly ISalesPersonBuilder _builder;
        private readonly ISalesPersonWriter _writer;

        public UpdateSalesPersonHandler(ISalesPersonBuilder builder,
            ISalesPersonWriter writer)
        {
            _builder = builder;
            _writer = writer;
        }

        public Task Handle(UpdateSalesPersonCommand request, CancellationToken cancellationToken)
        {
            //  GUARD
            Guard.Argument(() => request).NotNull()
                .Member(x => x.SalesPersonName, y => y.NotEmpty());

            //  BUILD
            _aggRoot = _builder
                .Load(request)
                .Name(request.SalesPersonName)
                .Build();

            //  APPLY
            _writer.Save(ref _aggRoot);
            return Task.FromResult(Unit.Value);
        }
    }
}