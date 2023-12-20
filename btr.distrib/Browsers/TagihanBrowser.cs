using btr.application.FinanceContext.TagihanAgg;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.domain.FinanceContext.TagihanAgg;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;

namespace btr.distrib.Browsers
{
    public class TagihanBrowser : IBrowser<TagihanBrowserView>
    {
        private readonly ITagihanDal _tagihanDal;

        public TagihanBrowser(ITagihanDal tagihanDal)
        {
            _tagihanDal = tagihanDal;
            Filter = new BrowseFilter();
            Filter.IsDate = true;
            Filter.HideAllRows = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new BrowserForm<TagihanBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<TagihanBrowserView> GenDataSource()
        {
            var listData = _tagihanDal.ListData(Filter.Date)?.ToList() ?? new List<TagihanModel>();
            var result = listData
                .OrderBy(x => x.TagihanDate)
                .Select(x => new TagihanBrowserView
                {
                    Id = x.TagihanId,
                    Tgl = x.TagihanDate.ToString("dd-MMM HH:mm"),
                    Sales = x.SalesPersonName,
                    TotalTagihan = x.TotalTagihan
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.Sales.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class TagihanBrowserView
    {
        public string Id { get; set; }
        public string Tgl { get; set; }
        public string Sales { get; set; }
        public decimal TotalTagihan { get; set; }
    }
}
