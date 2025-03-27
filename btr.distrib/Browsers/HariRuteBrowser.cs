using btr.application.SalesContext.RuteAgg;
using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.domain.SalesContext.HariRuteAgg;
using btr.domain.SalesContext.RuteAgg;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;

namespace btr.distrib.Browsers
{
    public class HariRuteBrowser :
        IBrowser<HariRuteBrowserView>,
        IBrowseEngine<HariRuteBrowserView>
    {
        private readonly IHariRuteDal _hariRuteDal;

        public HariRuteBrowser(IHariRuteDal hariRuteDal)
        {
            _hariRuteDal = hariRuteDal;
            Filter = new BrowseFilter();
            Filter.IsDate = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new BrowserForm<HariRuteBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<HariRuteBrowserView> GenDataSource()
        {
            var listData = _hariRuteDal.ListData()?.ToList() ?? new List<HariRuteModel>();

            var result = listData
                .OrderBy(x => x.HariRuteId)
                .Select(x => new HariRuteBrowserView
                {
                    Id = x.HariRuteId,
                    Name = x.HariRuteName,
                    ShortName= x.ShortName
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.Name.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class HariRuteBrowserView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName{ get; set; }
    }
}
