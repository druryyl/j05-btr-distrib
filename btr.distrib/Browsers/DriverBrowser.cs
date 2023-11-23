using btr.distrib.SharedForm;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;
using btr.distrib.Helpers;
using btr.application.InventoryContext.DriverAgg;
using btr.domain.InventoryContext.DriverAgg;

namespace btr.distrib.Browsers
{
    public class DriverBrowser : IBrowser<DriverBrowserView>
    {
        private readonly IDriverDal _driverDal;

        public DriverBrowser(IDriverDal driverDal)
        {
            _driverDal = driverDal;
            Filter = new BrowseFilter();
            Filter.IsDate = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new BrowserForm<DriverBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<DriverBrowserView> GenDataSource()
        {
            var listData = _driverDal.ListData()?.ToList() ?? new List<DriverModel>();

            var result = listData
                .OrderBy(x => x.DriverName)
                .Select(x => new DriverBrowserView
                {
                    Id = x.DriverId,
                    DriverName = x.DriverName
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.DriverName.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class DriverBrowserView
    {
        public string Id { get; set; }
        public string DriverName { get; set; }
    }
}
