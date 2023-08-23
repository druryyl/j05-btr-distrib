using btr.application.BrgContext.BrgStokViewAgg.Contracts;
using btr.distrib.SharedForm;
using btr.domain.BrgContext.BrgStokViewAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;
using btr.distrib.Helpers;

namespace btr.distrib.Browsers
{
    public class BrgStok2Browser :
        IBrowser<BrgStokBrowserView>,
        IBrowseEngine<BrgStokBrowserView>
    {
        private readonly IBrgStokViewDal _brgStokViewDal;

        public BrgStok2Browser(IBrgStokViewDal brgStokViewDal)
        {
            _brgStokViewDal = brgStokViewDal;
            Filter = new BrowseFilter();
            Filter.IsDate = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new Browser2Form<BrgStokBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<BrgStokBrowserView> GenDataSource()
        {
            var warehouse = new WarehouseModel(Filter.StaticFilter1);
            var listData = _brgStokViewDal.ListData(warehouse)?.ToList() 
                ?? new List<BrgStokViewModel>();

            var result = listData
                .OrderBy(x => x.BrgName)
                .Select(x => new BrgStokBrowserView
                {
                    Id = x.BrgId,
                    BrgName = x.BrgName,
                    Stok = x.Stok,
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.BrgName.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class BrgStokBrowserView
    {
        public string Id { get; set; }
        public string BrgName { get; set; }
        public int Stok { get; set; }
    }
}
