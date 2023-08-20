using System.Collections.Generic;

namespace btr.distrib.Browsers
{
    public interface IBrowseEngine<T>
    {
        BrowseFilter Filter { get; set; }
        IEnumerable<T> GenDataSource();
    }
}
