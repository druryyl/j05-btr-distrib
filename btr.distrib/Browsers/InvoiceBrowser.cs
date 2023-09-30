using btr.distrib.SharedForm;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;
using btr.distrib.Helpers;
using btr.application.PurchaseContext.InvoiceAgg;
using btr.domain.PurchaseContext.InvoiceAgg;

namespace btr.distrib.Browsers
{
    public class InvoiceBrowser : IBrowser<InvoiceBrowserView>
    {
        private readonly IInvoiceDal _invoiceDal;

        public InvoiceBrowser(IInvoiceDal invoiceDal)
        {
            _invoiceDal = invoiceDal;
            Filter = new BrowseFilter();
            Filter.IsDate = true;
            Filter.HideAllRows = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new BrowserForm<InvoiceBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<InvoiceBrowserView> GenDataSource()
        {
            var listData = _invoiceDal.ListData(Filter.Date)?.ToList() ?? new List<InvoiceModel>() ;
            var result = listData
                .OrderBy(x => x.SupplierName)
                .Select(x => new InvoiceBrowserView
                {
                    Id = x.InvoiceId,
                    Code = x.InvoiceCode,
                    Tgl = x.InvoiceDate.ToString("dd-MMM HH:mm"),
                    Supplier = x.SupplierName,
                    GrandTotal = x.GrandTotal
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.Supplier.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class InvoiceBrowserView
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Tgl { get; set; }
        public string Supplier { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
