using btr.application.FinanceContext.FpKeluaragAgg;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.domain.FinanceContext.FpKeluaranAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.Browsers
{
    public class FpKeluaranBrowser : IBrowser<FpKeluaranBrowserView>
    {
        private readonly IFpKeluaranDal _fpKeluaranDal;

        public FpKeluaranBrowser(IFpKeluaranDal fpKeluaranDal)
        {
            _fpKeluaranDal = fpKeluaranDal;
            Filter = new BrowseFilter();
            Filter.IsDate = true;
            Filter.HideAllRows = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new BrowserForm<FpKeluaranBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<FpKeluaranBrowserView> GenDataSource()
        {
            var listData = _fpKeluaranDal.ListData(Filter.Date)?.ToList()
                ?? new List<FpKeluaranModel>();

            var result = listData
                .OrderBy(x => x.FpKeluaranId)
                .Select(x => new FpKeluaranBrowserView
                {
                    Id = x.FpKeluaranId,
                    Tgl = x.UserDate.ToString("dd-MMM HH:mm"),
                    Keterangan = x.Keterangan,
                    FakturCount = x.FakturCount,
                    TotalPpn = x.TotalPpn
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.Id.ContainMultiWord(Filter.UserKeyword))
                    .ToList();

            return result;
        }
    }

    public class FpKeluaranBrowserView
    {
        public string Id { get; set; }
        public string Tgl { get; set; }
        public string Keterangan { get; set; }
        public int FakturCount { get; set; }
        public decimal TotalPpn { get; set; }
    }
}
