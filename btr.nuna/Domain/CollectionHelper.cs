using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.nuna.Domain
{
    public static class CollectionHelper
    {
        public static List<T> SafeToList<T>(this IEnumerable<T> source)
        {
            return source?.ToList() ?? new List<T>();
        }
    }
}
