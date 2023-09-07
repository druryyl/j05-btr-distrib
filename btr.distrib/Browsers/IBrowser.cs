using System.Collections.Generic;
using System.Threading.Tasks;
using btr.distrib.Helpers;
using btr.nuna.Domain;

namespace btr.distrib.Browsers
{
    public interface IQueryBrowser<TResult>
    {
        bool HideAllRow { get; }
        bool IsShowDate { get; }
        string[] BrowserQueryArgs { get; set; }
        Task<IEnumerable<TResult>> Browse(string userSearch, Periode userPeriode);
    }
    public interface IBrowser<T> : IBrowseEngine<T>
    {
        string Browse(string defaultValue);
    }
}
