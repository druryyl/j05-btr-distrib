using btr.distrib.SharedForm;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.distrib.Helpers;
using btr.domain.BrgContext.BrgAgg;

namespace btr.distrib.Browsers
{
    public class BrgBrowser : IBrowser<BrgBrowserView>
    {
        private readonly IBrgDal _brgDal;

        public BrgBrowser(IBrgDal brgDal)
        {
            _brgDal = brgDal;
            Filter = new BrowseFilter
            {
                IsDate = false
            };
        }

        public string Browse(string defaultValue)
        {
            var form = new BrowserForm<BrgBrowserView>(this);

            var dialogResult = form.ShowDialog();
            return dialogResult == System.Windows.Forms.DialogResult.OK 
                ? form.Result 
                : defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<BrgBrowserView> GenDataSource()
        {
            var listData = _brgDal.ListData()?.ToList() ?? new List<BrgModel>();

            var result = listData
                .OrderBy(x => x.BrgName)
                .Select(x => new BrgBrowserView
                {
                    Id = x.BrgId,
                    Code = x.BrgCode,
                    BrgName = x.BrgName
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
            {
                var resultName = result.Where(x => x.BrgName.ContainMultiWord(Filter.UserKeyword)).ToList();
                var resultId = result.Where(x => x.Id.ToLower().StartsWith(Filter.UserKeyword.ToLower())).ToList();
                var resultCode = result.Where(x => x.Code.ToLower().StartsWith(Filter.UserKeyword.ToLower())).ToList();
                result = resultName.Concat(resultId).Concat(resultCode).ToList();
            }

            return result;
        }
    }

    public class BrgBrowserView
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string BrgName { get; set; }
    }
}