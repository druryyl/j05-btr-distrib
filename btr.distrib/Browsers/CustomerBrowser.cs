﻿using btr.distrib.SharedForm;
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
            Filter.HideAllRows = true;
        }

        public string Browse(string defaultValue)
        {
            var form = new BrowserForm<CustomerBrowserView>(this);

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
                    Code = x.CustomerCode,
                    CustomerName = $"{x.CustomerName} - {x.Address1} {x.Kota}",
                }).ToList();

            if (Filter.UserKeyword.Length > 0)
            {
                var resultName = result.Where(x => x.CustomerName.ContainMultiWord(Filter.UserKeyword)).ToList();
                var resultId = result.Where(x => x.Id.ToLower().StartsWith(Filter.UserKeyword.ToLower())).ToList();
                var resultCode = result.Where(x => x.Code.ToLower().StartsWith(Filter.UserKeyword.ToLower())).ToList();
                result = resultName
                    .Union(resultId)
                    .Union(resultCode)
                    .ToList();
            }
            return result;
        }
    }

    public class CustomerBrowserView
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
    }
}
