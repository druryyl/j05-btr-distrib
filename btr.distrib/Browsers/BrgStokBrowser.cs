using btr.application.InventoryContext.StokAgg.UseCases;
using btr.distrib.SharedForm;
using btr.nuna.Domain;
using MediatR;
using Polly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace btr.distrib.Browsers
{
    public interface IBrgStokBrowser : IQueryBrowser<ListBrgStokResponse>
    {
    }

    public class BrgStokBrowser : IBrgStokBrowser
    {
        private readonly IMediator _mediator;

        public BrgStokBrowser(IMediator mediator)
        {
            _mediator = mediator;
        }

        public bool IsShowDate { get; private set; }
        public string[] BrowserQueryArgs { get; set; }

        public async Task<IEnumerable<ListBrgStokResponse>> Browse(string userSearch, Periode userPeriode)
        {
            var brgName = userSearch;
            var warehouseId = BrowserQueryArgs[0];
            IsShowDate = false;

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
