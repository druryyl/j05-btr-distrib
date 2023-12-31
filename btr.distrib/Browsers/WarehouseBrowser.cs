﻿using btr.distrib.SharedForm;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;
using btr.application.InventoryContext.WarehouseAgg;
using btr.distrib.Helpers;

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
            var form = new BrowserForm<WarehouseBrowserView>(this);

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
                .Where(x => x.IsSpecial == false)
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
