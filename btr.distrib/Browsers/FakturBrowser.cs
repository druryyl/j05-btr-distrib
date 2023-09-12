using btr.distrib.SharedForm;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;
using btr.distrib.Helpers;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.domain.SalesContext.FakturAgg;

namespace btr.distrib.Browsers
{
    public class FakturBrowser : IBrowser<Faktur2BrowserView>
    {
        private readonly IFakturDal _fakturDal;

        public FakturBrowser(IFakturDal fakturDal)
        {
            _fakturDal = fakturDal;
            Filter = new BrowseFilter();
            Filter.IsDate = true;
            Filter.HideAllRows = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new BrowserForm<Faktur2BrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<Faktur2BrowserView> GenDataSource()
        {
            var listData = _fakturDal.ListData(Filter.Date)?.ToList() ?? new List<FakturModel>();
            var result = listData
                .OrderBy(x => x.CustomerName)
                .Select(x => new Faktur2BrowserView
                {
                    Id = x.FakturId,
                    Code = x.FakturCode,
                    Tgl = x.FakturDate.ToString("dd-MMM HH:mm"),
                    Customer = x.CustomerName,
                    Sales = x.SalesPersonName,
                    GrandTotal = x.GrandTotal
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.Customer.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class Faktur2BrowserView
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Tgl { get; set; }
        public string Customer { get; set; }
        public string Sales { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
