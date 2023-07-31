using System.Threading;
using System.Threading.Tasks;
using btr.application.SalesContext.SalesPersonAgg.Workers;
using btr.domain.SalesContext.SalesPersonAgg;
using Dawn;
using Mapster;
using MediatR;

namespace btr.application.SalesContext.SalesPersonAgg.UseCases
{
    public class CreateSalesPersonCommand : IRequest<CreateSalesPersonResponse>
    {
        public CreateSalesPersonCommand(string name) => SalesPersonName = name;
        public string SalesPersonName { get; }
    }

    public class CreateSalesPersonResponse
    {
        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
    }

    public class CreateSalesPersonHandler : IRequestHandler<CreateSalesPersonCommand, CreateSalesPersonResponse>
    {
        private SalesPersonModel _aggRoot = new SalesPersonModel();
        private readonly ISalesPersonBuilder _builder;
        private readonly ISalesPersonWriter _writer;

        public CreateSalesPersonHandler(ISalesPersonBuilder builder,
            ISalesPersonWriter writer)
        {
            _builder = builder;
            _writer = writer;
        }

        public Task<CreateSalesPersonResponse> Handle(CreateSalesPersonCommand request,
            CancellationToken cancellationToken)
        {
            //  GUARD
            Guard.Argument(() => request).NotNull()
                .Member(x => x.SalesPersonName, y => y.NotEmpty());

            //  BUILD
            _aggRoot = _builder
                .CreateNew()
                .Name(request.SalesPersonName)
                .Build();

            //  APPLY
            _writer.Save(ref _aggRoot);
            return Task.FromResult(GenResponse());
        }

        private CreateSalesPersonResponse GenResponse()
        {
            var result = _aggRoot.Adapt<CreateSalesPersonResponse>();
            return result;
        }
    }
}