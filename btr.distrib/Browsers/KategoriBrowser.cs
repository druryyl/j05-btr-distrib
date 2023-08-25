using btr.application.BrgContext.KategoriAgg.Contracts;
using btr.distrib.SharedForm;
using btr.domain.BrgContext.KategoriAgg;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;
using btr.distrib.Helpers;

namespace btr.distrib.Browsers
{
    public class KategoriBrowser : IBrowser<KategoriBrowserView>
    {
        private readonly IKategoriDal _kategoriDal;

        public KategoriBrowser(IKategoriDal kategoriDal)
        {
            _kategoriDal = kategoriDal;
            Filter = new BrowseFilter();
            Filter.IsDate = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new Browser2Form<KategoriBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<KategoriBrowserView> GenDataSource()
        {
            var listData = _kategoriDal.ListData()?.ToList() ?? new List<KategoriModel>();

            var result = listData
                .OrderBy(x => x.KategoriName)
                .Select(x => new KategoriBrowserView
                {
                    Id = x.KategoriId,
                    KategoriName = x.KategoriName
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.KategoriName.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class KategoriBrowserView
    {
        public string Id { get; set; }
        public string KategoriName { get; set; }
    }
}
