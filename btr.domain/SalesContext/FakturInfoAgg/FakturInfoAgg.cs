﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.SalesContext.InfoFakturAgg
{
    public class FakturInfoDto
    {
        public string FakturId { get; set; }
        public string FakturCode { get; set; }
        public string Tgl { get; set; }
        public string Admin { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string Address { get; set; }
        public string WilayahName { get; set; }
        public string SalesPersonName { get; set; }
        public decimal Total { get; set; }
        public decimal Diskon { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
    }
}