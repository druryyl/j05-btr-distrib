using btr.distrib.SharedForm;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;
using btr.distrib.Helpers;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.domain.SalesContext.SalesPersonAgg;

namespace btr.distrib.Browsers
{
    public class SalesPersonBrowser : IBrowser<SalesPersonBrowserView>
    {
        private readonly ISalesPersonDal _salesPersonDal;

        public SalesPersonBrowser(ISalesPersonDal salesPersonDal)
        {
            _salesPersonDal = salesPersonDal;
            Filter = new BrowseFilter();
            Filter.IsDate = false;
            Filter.HideAllRows = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new Browser2Form<SalesPersonBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<SalesPersonBrowserView> GenDataSource()
        {
            var listData = _salesPersonDal.ListData()?.ToList() ?? new List<SalesPersonModel>();

            var result = listData
                .OrderBy(x => x.SalesPersonName)
                .Select(x => new SalesPersonBrowserView
                {
                    Id = x.SalesPersonId,
                    SalesPersonName = x.SalesPersonName
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.SalesPersonName.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class SalesPersonBrowserView
    {
        public string Id { get; set; }
        public string SalesPersonName { get; set; }
    }
}
