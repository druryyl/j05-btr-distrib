using btr.distrib.SharedForm;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;
using btr.distrib.Helpers;
using btr.application.PurchaseContext.ReturBeliFeature;
using btr.domain.PurchaseContext.ReturBeliFeature;

namespace btr.distrib.Browsers
{
    public class ReturBeliBrowser : IBrowser<ReturBeliBrowserView>
    {
        private readonly IReturBeliDal _returBeliDal;

        public ReturBeliBrowser(IReturBeliDal returBeliDal)
        {
            _returBeliDal = returBeliDal;
            Filter = new BrowseFilter();
            Filter.IsDate = true;
            Filter.HideAllRows = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new BrowserForm<ReturBeliBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<ReturBeliBrowserView> GenDataSource()
        {
            var listData = _returBeliDal.ListData(Filter.Date)?.ToList() ?? new List<ReturBeliModel>() ;
            var result = listData
                .OrderBy(x => x.SupplierName)
                .Select(x => new ReturBeliBrowserView
                {
                    Id = x.ReturBeliId,
                    Code = x.ReturBeliCode,
                    Tgl = x.ReturBeliDate.ToString("dd-MMM HH:mm"),
                    Supplier = x.SupplierName,
                    GrandTotal = x.GrandTotal
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.Supplier.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class ReturBeliBrowserView
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Tgl { get; set; }
        public string Supplier { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
