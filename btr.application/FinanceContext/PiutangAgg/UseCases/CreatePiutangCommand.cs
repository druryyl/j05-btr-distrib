using System;
using System.Threading;
using System.Threading.Tasks;
using btr.application.FinanceContext.PiutangAgg.Workers;
using MediatR;

namespace btr.application.FinanceContext.PiutangAgg.UseCases
{
    public class CreatePiutangCommand : IRequest
    {
        public string PiutangId { get; set; }
        public DateTime PiutangDate { get; set; }
        public DateTime JatuhTempo { get; set; }
        public string CustomerId { get; set; }
        public decimal Total { get; set; }
    }

    public class CreatePiutangHandler : IRequestHandler<CreatePiutangCommand>
    {
        private readonly IPiutangBuilder _builder;

        public CreatePiutangHandler(IPiutangBuilder builder)
        {
            _builder = builder;
        }

        public Task Handle(CreatePiutangCommand request, CancellationToken cancellationToken)
        {
            //  GUARD
        }
    }
}