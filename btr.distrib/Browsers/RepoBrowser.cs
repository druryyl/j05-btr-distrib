using btr.distrib.Helpers;
using btr.distrib.SharedForm;
using btr.infrastructure.SupportContext.RoleFeature;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;

namespace btr.distrib.Browsers
{
    public class RoleBrowser : IBrowser<RoleBrowserView>
    {
        private readonly IRoleDal _roleDal;

        public RoleBrowser(IRoleDal roleDal)
        {
            _roleDal = roleDal;
            Filter = new BrowseFilter();
            Filter.IsDate = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new BrowserForm<RoleBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<RoleBrowserView> GenDataSource()
        {
            var listData = _roleDal.ListData()?.ToList() ?? new List<RoleDto>();

            var result = listData
                .OrderBy(x => x.RoleName)
                .Select(x => new RoleBrowserView
                {
                    Id = x.RoleId,
                    RoleName = x.RoleName
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.RoleName.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class RoleBrowserView
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
    }
}
