using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.SharedForm
{
    public interface IDateBrowser<T>
    {
        Task<IEnumerable<T>> Browse(Periode periode);
    }
    public interface IStringBrowser<T>
    {
        Task<IEnumerable<T>> Browse(string keyword, string filter);
    }
}
