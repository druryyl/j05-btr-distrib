using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.distrib.SharedForm;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;

namespace btr.distrib.Browsers
{
    public class SupplierBrowser : 
        IBrowser<SupplierBrowserView>,
        IBrowseEngine<SupplierBrowserView>
    {
        private readonly ISupplierDal _supplierDal;

        public SupplierBrowser(ISupplierDal supplierDal)
        {
            _supplierDal = supplierDal;
            Filter = new BrowseFilter();
            Filter.IsDate = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new Browser2Form<SupplierBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<SupplierBrowserView> GenDataSource()
        {
            var listData = _supplierDal.ListData()?.ToList() ?? new List<SupplierModel>();

            var result = listData
                .OrderBy(x => x.SupplierName)
                .Select(x => new SupplierBrowserView
                {
                    Id = x.SupplierId,
                    SupplierName = x.SupplierName
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.SupplierName.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class SupplierBrowserView
    {
        public string Id { get; set; }
        public string SupplierName { get; set; }
    }
}
