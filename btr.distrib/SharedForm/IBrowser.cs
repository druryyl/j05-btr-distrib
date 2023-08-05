using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.SharedForm
{
    public interface IBrowser<TResult>
    {
        Task<IEnumerable<TResult>> Browse(string userSearch, Periode userPeriode, string[] args);
    }
}
