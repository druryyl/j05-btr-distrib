using btr.distrib.SharedForm;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;
using btr.distrib.Helpers;
using btr.application.SalesContext.WilayahAgg;
using btr.domain.SalesContext.SalesPersonAgg;

namespace btr.distrib.Browsers
{
    public class WilayahBrowser : IBrowser<WilayahBrowserView>
    {
        private readonly IWilayahDal _wilayahDal;

        public WilayahBrowser(IWilayahDal wilayahDal)
        {
            _wilayahDal = wilayahDal;
            Filter = new BrowseFilter();
            Filter.IsDate = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new Browser2Form<WilayahBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<WilayahBrowserView> GenDataSource()
        {
            var listData = _wilayahDal.ListData()?.ToList() ?? new List<WilayahModel>();

            var result = listData
                .OrderBy(x => x.WilayahName)
                .Select(x => new WilayahBrowserView
                {
                    Id = x.WilayahId,
                    WilayahName = x.WilayahName
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.WilayahName.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class WilayahBrowserView
    {
        public string Id { get; set; }
        public string WilayahName { get; set; }
    }
}
