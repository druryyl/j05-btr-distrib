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
            var form = new BrowserForm<SalesPersonBrowserView>(this);

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
                    Code = x.SalesPersonCode,
                    SalesPersonName = x.SalesPersonName
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
            {
                var resultName = result.Where(x => x.SalesPersonName.ContainMultiWord(Filter.UserKeyword)).ToList();
                var resultId = result.Where(x => x.Id.ToLower().StartsWith(Filter.UserKeyword.ToLower())).ToList();
                var resultCode = result.Where(x => x.Code.ToLower().StartsWith(Filter.UserKeyword.ToLower())).ToList();
                result = resultName.Concat(resultId).Concat(resultCode).ToList();
            }
                
            return result;
        }
    }

    public class SalesPersonBrowserView
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string SalesPersonName { get; set; }
    }
}
