using btr.nuna.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace btr.distrib.SharedForm
{
    public interface IBrowser<TResult>
    {
        bool IsShowDate { get; }
        string[] BrowserQueryArgs { get; set; }
        Task<IEnumerable<TResult>> Browse(string userSearch, Periode userPeriode);
    }
}
