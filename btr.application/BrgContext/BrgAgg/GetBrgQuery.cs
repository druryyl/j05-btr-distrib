using System.Threading;
using System.Threading.Tasks;
using btr.domain.BrgContext.BrgAgg;
using Dawn;
using MediatR;

namespace btr.application.BrgContext.BrgAgg
{

    public class GetBrgQuery : IRequest<BrgModel>, IBrgKey
    {
        public GetBrgQuery(string brgId) => BrgId = brgId;
        public string BrgId { get; }
    }


    public class GetBrgHargaHandler : IRequestHandler<GetBrgQuery, BrgModel>
    {
        private BrgModel _aggRoot = new BrgModel();
        private readonly IBrgBuilder _builder;

        public GetBrgHargaHandler(IBrgBuilder builder)
        {
            _builder = builder;
        }

        public Task<BrgModel> Handle(GetBrgQuery request, CancellationToken cancellationToken)
        {
            //  GUARD
            Guard.Argument(() => request)
                .Member(x => x.BrgId, y => y.NotEmpty());

            //  QUERY
            _aggRoot = _builder
                .Load(request)
                .Build();

            //  RESPONSE
            return Task.FromResult(_aggRoot);
        }
    }
}