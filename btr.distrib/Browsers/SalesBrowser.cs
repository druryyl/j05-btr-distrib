using btr.distrib.SharedForm;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;
using btr.distrib.Helpers;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.domain.SalesContext.SalesPersonAgg;

namespace btr.distrib.Browsers
{
    public class SalesBrowser : IBrowser<SalesBrowserView>
    {
        private readonly ISalesPersonDal _salesDal;

        public SalesBrowser(ISalesPersonDal salesDal)
        {
            _salesDal = salesDal;
            Filter = new BrowseFilter();
            Filter.IsDate = false;
            Filter.HideAllRows = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new Browser2Form<SalesBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<SalesBrowserView> GenDataSource()
        {
            var listData = _salesDal.ListData()?.ToList() ?? new List<SalesPersonModel>();

            var result = listData
                .OrderBy(x => x.SalesPersonName)
                .Select(x => new SalesBrowserView
                {
                    Id = x.SalesPersonId,
                    SalesName = x.SalesPersonName
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.SalesName.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class SalesBrowserView
    {
        public string Id { get; set; }
        public string SalesName { get; set; }
    }
}
