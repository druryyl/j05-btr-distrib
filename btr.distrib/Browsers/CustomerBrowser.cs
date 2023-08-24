using btr.distrib.SharedForm;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Linq;
using btr.distrib.Helpers;
using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.domain.SalesContext.CustomerAgg;

namespace btr.distrib.Browsers
{
    public class CustomerBrowser : IBrowser<CustomerBrowserView>
    {
        private readonly ICustomerDal _customerDal;

        public CustomerBrowser(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
            Filter = new BrowseFilter();
            Filter.IsDate = false;
        }

        public string Browse(string defaultValue)
        {
            var form = new Browser2Form<CustomerBrowserView>(this);

            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.Result;
            else
                return defaultValue;
        }

        public BrowseFilter Filter { get; set; }

        public IEnumerable<CustomerBrowserView> GenDataSource()
        {
            var listData = _customerDal.ListData()?.ToList() ?? new List<CustomerModel>();

            var result = listData
                .OrderBy(x => x.CustomerName)
                .Select(x => new CustomerBrowserView
                {
                    Id = x.CustomerId,
                    CustomerName = x.CustomerName
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
                result = result
                    .Where(x => x.CustomerName.ContainMultiWord(Filter.UserKeyword)).ToList();

            return result;
        }
    }

    public class CustomerBrowserView
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
    }
}
