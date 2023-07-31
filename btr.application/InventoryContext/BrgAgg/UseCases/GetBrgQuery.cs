using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using btr.application.InventoryContext.BrgAgg.Workers;
using btr.domain.InventoryContext.BrgAgg;
using Dawn;
using Mapster;
using MediatR;

namespace btr.application.InventoryContext.BrgAgg.UseCases
{

    public class GetBrgHargaQuery : IRequest<GetBrgHargaResponse>, IBrgKey
    {
        public GetBrgHargaQuery(string brgId) => BrgId = brgId;
        public string BrgId { get; }
    }

    public class GetBrgHargaResponse
    {
        public string BrgId { get; set; }
        public string BrgName { get; set;}
        public List<GetBrgHargaResponseSatuanHrg> ListSatuanHarga { get; set; }
    }

    public class GetBrgHargaResponseSatuanHrg
    {
        public string BrgId { get; set; }
        public string Satuan { get; set; }
        public int Conversion { get; set; }
        public int Stok { get; set; }
        public double HargaJual { get; set; }
    }

    public class GetBrgHargaHandler : IRequestHandler<GetBrgHargaQuery, GetBrgHargaResponse>
    {
        private BrgModel _aggRoot = new BrgModel();
        private IBrgBuilder _builder;

        public GetBrgHargaHandler(IBrgBuilder builder)
        {
            _builder = builder;
        }

        public Task<GetBrgHargaResponse> Handle(GetBrgHargaQuery request, CancellationToken cancellationToken)
        {
            //  GUARD
            Guard.Argument(() => request)
                .Member(x => x.BrgId, y => y.NotEmpty());
            
            //  QUERY
            _aggRoot = _builder
                .Load(request)
                .Build();
            
            //  RESPONSE
            return Task.FromResult(GenResponse());
        }

        private GetBrgHargaResponse GenResponse()
        {
            var result = _aggRoot.Adapt<GetBrgHargaResponse>();
            return result;
        }
    }
}