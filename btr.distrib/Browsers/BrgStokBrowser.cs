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
    public class BrgStokBrowser :
        IBrowser<BrgStokBrowserView>,
        IBrowseEngine<BrgStokBrowserView>
    {
        private readonly IBrgStokViewDal _brgStokViewDal;

        public BrgStokBrowser(IBrgStokViewDal brgStokViewDal)
        {
            _brgStokViewDal = brgStokViewDal;
            Filter = new BrowseFilter();
            Filter.IsDate = false;
            Filter.HideAllRows = true;
        }

        public string Browse(string defaultValue)
        {
            var form = new BrowserForm<BrgStokBrowserView>(this);

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
                    Code = x.BrgCode,
                    BrgName = x.BrgName,
                    Stok = x.Stok,
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
            {
                var resultName = result.Where(x => x.BrgName.ContainMultiWord(Filter.UserKeyword)).ToList();
                var resultId = result.Where(x => x.Id.ToLower().StartsWith(Filter.UserKeyword.ToLower())).ToList();
                var resultCode = result.Where(x => x.Code.ToLower().StartsWith(Filter.UserKeyword.ToLower())).ToList();
                result = resultName.Concat(resultId).Concat(resultCode).ToList();
            }

            return result;
        }
    }

    public class BrgStokBrowserView
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string BrgName { get; set; }
        public int Stok { get; set; }
    }
}
