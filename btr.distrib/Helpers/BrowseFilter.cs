using btr.nuna.Domain;
using System;

namespace btr.distrib.Browsers
{
    public class BrowseFilter
    {
        public string UserKeyword { get; set; }
        public bool IsDate { get; set; }
        public Periode Date { get; set; }

        public BrowseFilter()
        {
            UserKeyword = null;
            IsDate = false;
            Date = new Periode(DateTime.MinValue);
        }
    }
}
