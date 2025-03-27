using btr.application.InventoryContext.WarehouseAgg;
using btr.application.SalesContext.RuteAgg;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.RuteAgg;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.Browsers
{
    public class RuteBrowser :
        IBrowser<RuteBrowserView>,
        IBrowseEngine<RuteBrowserView>
    {
        private readonly IHariRuteDal _ruteDal;

        public RuteBrowser(IHariRuteDal ruteDal)
        {
            _ruteDal = ruteDal;
            Filter = new BrowseFilter();
            Filter.IsDate = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new BrowserForm<RuteBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<RuteBrowserView> GenDataSource()
        {
            var listData = _ruteDal.ListData()?.ToList() ?? new List<RuteModel>();

            var result = listData
                .OrderBy(x => x.RuteCode)
                .Select(x => new RuteBrowserView
                {
                    Id = x.RuteId,
                    Code = x.RuteCode,
                    RuteNAme = x.RuteName
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.RuteNAme.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class RuteBrowserView
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string RuteNAme{ get; set; }
    }
}
