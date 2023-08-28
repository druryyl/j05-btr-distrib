using System;
using btr.nuna.Domain;

namespace btr.distrib.Helpers
{
    public class BrowseFilter
    {
        public string UserKeyword { get; set; }
        public bool IsDate { get; set; }
        public Periode Date { get; set; }
        public bool HideAllRows { get; set; }
        
        public string StaticFilter1 { get; set; }
        public string StaticFilter2 { get; set; }

        public BrowseFilter()
        {
            UserKeyword = null;
            IsDate = false;
            Date = new Periode(DateTime.MinValue);
        }
    }
}
