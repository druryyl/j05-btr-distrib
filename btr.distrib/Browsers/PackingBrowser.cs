using btr.distrib.SharedForm;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;
using btr.application.InventoryContext.PackingAgg;
using btr.distrib.Helpers;
using btr.domain.InventoryContext.PackingAgg;

namespace btr.distrib.Browsers
{
    public class PackingBrowser : IBrowser<PackingBrowserView>
    {
        private readonly IPackingDal _packingDal;

        public PackingBrowser(IPackingDal packingDal)
        {
            _packingDal = packingDal;
            Filter = new BrowseFilter
            {
                IsDate = true,
                HideAllRows = false
            };
        }

        public string Browse(string defaultValue)
        {
            var form = new BrowserForm<PackingBrowserView>(this);

            var dialogResult = form.ShowDialog();
            return dialogResult == System.Windows.Forms.DialogResult.OK 
                ? form.Result 
                : defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<PackingBrowserView> GenDataSource()
        {
            var listData = _packingDal.ListData(Filter.Date)?.ToList() ?? new List<PackingModel>();
            var result = listData
                .OrderBy(x => x.PackingId)
                .Select(x => new PackingBrowserView
                {
                    Id = x.PackingId,
                    Tgl = x.PackingDate.ToString("dd-MMM HH:mm"),
                    Driver = x.DriverName,
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.Driver.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class PackingBrowserView
    {
        public string Id { get; set; }
        public string Tgl { get; set; }
        public string Driver { get; set; }
    }
}
