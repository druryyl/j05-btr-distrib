using btr.application.InventoryContext.WarehouseAgg.Contracts;
using btr.distrib.SharedForm;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;

namespace btr.distrib.Browsers
{
    public class WarehouseBrowser :
        IBrowser<WarehouseBrowserView>,
        IBrowseEngine<WarehouseBrowserView>
    {
        private readonly IWarehouseDal _warehouseDal;

        public WarehouseBrowser(IWarehouseDal warehouseDal)
        {
            _warehouseDal = warehouseDal;
            Filter = new BrowseFilter();
            Filter.IsDate = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new Browser2Form<WarehouseBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<WarehouseBrowserView> GenDataSource()
        {
            var listData = _warehouseDal.ListData()?.ToList() ?? new List<WarehouseModel>();

            var result = listData
                .OrderBy(x => x.WarehouseName)
                .Select(x => new WarehouseBrowserView
                {
                    Id = x.WarehouseId,
                    WarehouseName = x.WarehouseName
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.WarehouseName.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class WarehouseBrowserView
    {
        public string Id { get; set; }
        public string WarehouseName { get; set; }
    }
}
