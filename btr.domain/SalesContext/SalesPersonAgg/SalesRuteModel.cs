﻿using btr.domain.SalesContext.CustomerAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.SalesContext.SalesPersonAgg
{
    public class SalesRuteModel : ISalesRuteKey, ISalesPersonKey
    {
        public SalesRuteModel()
        {
        }
        public SalesRuteModel(string id)
        {
            SalesRuteId = id;
        }
        public string SalesRuteId { get; set; }
        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
        public string HariRuteId { get; set; }
        public string HariRuteName { get; set; }
        public string ShortName { get; set; }
        public List<SalesRuteItemModel> ListCustomer { get; set; }
    }

    public class SalesRuteItemModel : ISalesRuteKey, ICustomerKey
    {
        public string SalesRuteId { get; set; }
        public int NoUrut { get; set; }
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Wilayah { get; set; }
    }
}
