using btr.application.InventoryContext.StokAgg.UseCases;
using btr.distrib.SharedForm;
using btr.nuna.Domain;
using MediatR;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.Browsers
{
    public interface IBrgStok2Browser : IBrowser
    {
        IBrgStok2Browser Warehouse(string warehouseId);
    }

    public class BrgStok2Browser : IBrgStok2Browser
    {
        private string _warehouseId = string.Empty;
        private readonly IQueryBrowser<BrgStok2BrowseProjection> _queryBrowser;

        public BrgStok2Browser(IQueryBrowser<BrgStok2BrowseProjection> queryBrowser)
        {
            _queryBrowser = queryBrowser;
        }

        public IBrgStok2Browser Warehouse(string warehouseId)
        {
            _warehouseId = warehouseId;
            return this;
        }

        public string Browse(string defaultValue)
        {
            var form = new BrowserForm<BrgStok2BrowseProjection, string>(_queryBrowser, defaultValue, x => x.BrgName);
            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.ReturnedValue;
            else
                return defaultValue;
        }
    }

    public class BrgStok2BrowseProjection
    {
        public string Id { get; set; }
        public string BrgName { get; set; }
        public int Stok { get; set; }
    }

    public class BrgStok2QueryBrowser : IQueryBrowser<BrgStok2BrowseProjection>
    {
        public bool IsShowDate { get; private set; }
        //  TODO: error di sini; susah passingkan filter warehouseId
        //      coba akses langsung DAL; buatkan Dal baru sesuai kebutuhan query
        public string[] BrowserQueryArgs { get; set; }
        private readonly IMediator _mediator;

        public BrgStok2QueryBrowser(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<BrgStok2BrowseProjection>> Browse(string userSearch, Periode userPeriode)
        {
            var brgName = userSearch;
            var warehouseId = BrowserQueryArgs[0];
            IsShowDate = false;

            var policy = Policy<IEnumerable<ListBrgStokResponse>>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .FallbackAsync(new List<ListBrgStokResponse>());

            var query = new ListBrgStokQuery(brgName, warehouseId);
            Task<IEnumerable<ListBrgStokResponse>> queryTask() => _mediator.Send(query);
            var response = await policy.ExecuteAsync(queryTask);
            var listBrgStok2 =
                from c in response
                select new BrgStok2BrowseProjection
                {
                    Id = c.BrgId,
                    BrgName = c.BrgName,
                    Stok = c.Qty
                };
            return listBrgStok2;
        }
    }
}
