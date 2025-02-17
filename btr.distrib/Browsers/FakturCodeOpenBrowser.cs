using btr.application.InventoryContext.DriverAgg;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.domain.InventoryContext.DriverAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.Browsers
{
    public class FakturCodeOpenBrowser : IBrowser<FakturCodeOpenBrowserView>
    {
        private readonly IFakturCodeOpenDal _fakturCodeOpenDal;

        public FakturCodeOpenBrowser(IFakturCodeOpenDal fakturCodeOpenDal)
        {
            _fakturCodeOpenDal = fakturCodeOpenDal;
            Filter = new BrowseFilter();
            Filter.IsDate = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new BrowserForm<FakturCodeOpenBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<FakturCodeOpenBrowserView> GenDataSource()
        {
            var listData = _fakturCodeOpenDal.ListData()?.ToList() ?? new List<string>();

            var result = listData
                .OrderBy(x => x)
                .Select(x => new FakturCodeOpenBrowserView
                {
                    FakturCode = x,
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.FakturCode.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class FakturCodeOpenBrowserView
    {
        public string FakturCode{ get; set; }
    }
}
