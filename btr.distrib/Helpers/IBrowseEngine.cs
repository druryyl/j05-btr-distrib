using System.Collections.Generic;

namespace btr.distrib.Helpers
{
    public interface IBrowseEngine<T>
    {
        BrowseFilter Filter { get; set; }
        IEnumerable<T> GenDataSource();
    }
}
