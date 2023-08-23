using btr.distrib.Browsers;
using btr.nuna.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using btr.distrib.Helpers;

namespace btr.distrib.SharedForm
{
    public interface IQueryBrowser<TResult>
    {
        bool IsShowDate { get; }
        string[] BrowserQueryArgs { get; set; }
        Task<IEnumerable<TResult>> Browse(string userSearch, Periode userPeriode);
    }
    public interface IBrowser
    {
        string Browse(string defaultValue);
    }

    public interface IBrowser<T> : IBrowseEngine<T>
    {
        string Browse(string defaultValue);
    }
}
