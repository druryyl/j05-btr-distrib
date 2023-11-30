using btr.distrib.SharedForm;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;
using btr.distrib.Helpers;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.domain.SalesContext.FakturAgg;
using btr.application.InventoryContext.ReturJualAgg.Contracts;
using btr.domain.InventoryContext.ReturJualAgg;

namespace btr.distrib.Browsers
{
    public class ReturJualBrowser : IBrowser<ReturJualBrowserView>
    {
        private readonly IReturJualDal _returJualDal;

        public ReturJualBrowser(IReturJualDal returJualDal)
        {
            _returJualDal = returJualDal;
            Filter = new BrowseFilter();
            Filter.IsDate = true;
            Filter.HideAllRows = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new BrowserForm<ReturJualBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<ReturJualBrowserView> GenDataSource()
        {
            var listData = _returJualDal.ListData(Filter.Date)?.ToList() ?? new List<ReturJualModel>();
            var result = listData
                .OrderBy(x => x.CustomerName)
                .Select(x => new ReturJualBrowserView
                {
                    Id = x.ReturJualId,
                    Tgl = x.ReturJualDate.ToString("dd-MMM HH:mm"),
                    Customer = x.CustomerName,
                    GrandTotal = x.GrandTotal
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.Customer.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class ReturJualBrowserView
    {
        public string Id { get; set; }
        public string Tgl { get; set; }
        public string Customer { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
