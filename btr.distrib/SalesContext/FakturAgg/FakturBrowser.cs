using btr.application.SalesContext.FakturAgg.UseCases;
using btr.distrib.SharedForm;
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
    public interface IFakturBrowser : IBrowser<ListFakturResponse>
    {
    }

    public class FakturBrowser : IFakturBrowser
    {
        private readonly IMediator _mediator;

        public FakturBrowser(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<ListFakturResponse>> Browse(string userSearch, Periode periode, string[] args)
        {
            var tgl1 = periode.Tgl1.ToString("yyyy-MM-dd");
            var tgl2 = periode.Tgl2.ToString("yyyy-MM-dd");

            var policy = Policy<IEnumerable<ListFakturResponse>>
                .Handle<KeyNotFoundException>()
                .FallbackAsync(new List<ListFakturResponse>());
            var query = new ListFakturQuery(tgl1, tgl2);
            Task<IEnumerable<ListFakturResponse>> queryTask() => _mediator.Send(query);
            var result = await policy.ExecuteAsync(queryTask);
            return result;
        }
    }
}
