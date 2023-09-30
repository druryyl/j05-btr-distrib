using btr.distrib.SharedForm;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;
using btr.distrib.Helpers;
using btr.application.InventoryContext.MutasiAgg;
using btr.domain.InventoryContext.MutasiAgg;

namespace btr.distrib.Browsers
{
    public class MutasiBrowser : IBrowser<MutasiBrowserView>
    {
        private readonly IMutasiDal _mutasiDal;

        public MutasiBrowser(IMutasiDal mutasiDal)
        {
            _mutasiDal = mutasiDal;
            Filter = new BrowseFilter();
            Filter.IsDate = true;
            Filter.HideAllRows = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new BrowserForm<MutasiBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<MutasiBrowserView> GenDataSource()
        {
            var listData = _mutasiDal.ListData(Filter.Date)?.ToList() ?? new List<MutasiModel>();
            var result = listData
                .OrderBy(x => x.MutasiId)
                .Select(x => new MutasiBrowserView
                {
                    Id = x.MutasiId,
                    Tgl = x.MutasiDate.ToString("dd-MMM HH:mm"),
                    User = x.UserId,
                    Warehouse = x.WarehouseName,
                    NilaiSediaan = x.NilaiSediaan
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.User.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class MutasiBrowserView
    {
        public string Id { get; set; }
        public string Tgl { get; set; }
        public string User { get; set; }
        public string Warehouse{ get; set; }
        public decimal NilaiSediaan { get; set; }
    }
}
