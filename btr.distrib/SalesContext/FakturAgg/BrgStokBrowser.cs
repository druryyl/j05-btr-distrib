using btr.application.InventoryContext.StokAgg.UseCases;
using btr.application.SalesContext.FakturAgg.UseCases;
using btr.distrib.SharedForm;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Domain;
using MediatR;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.SalesContext.FakturAgg
{
    public interface IBrgStokBrowser : IBrowser<ListBrgStokResponse>
    {
    }

    public class BrgStokBrowser : IBrgStokBrowser
    {
        private readonly IMediator _mediator;

        public BrgStokBrowser(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<ListBrgStokResponse>> Browse(string userSearch, Periode userPeriode, string[] args)
        {
            var brgName = userSearch;
            var warehouseId = args[0];

            var policy = Policy<IEnumerable<ListBrgStokResponse>>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .FallbackAsync(new List<ListBrgStokResponse>());

            var query = new ListBrgStokQuery(brgName, warehouseId);
            Task<IEnumerable<ListBrgStokResponse>> queryTask() => _mediator.Send(query);
            var result = await policy.ExecuteAsync(queryTask);
            return result;
        }
    }
}
