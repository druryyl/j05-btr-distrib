using System.Collections.Generic;
using System.Linq;

namespace btr.nuna.Domain
{
    public static class NunaModelHelper
    {
        public static void RemoveNull<T>(this T obj)
        {
            if (EqualityComparer<T>.Default.Equals(obj, default))
            {
                return;
            }
            var properties = from p in typeof(T).GetProperties()
                where p.PropertyType == typeof(string) &&
                      p.CanRead &&
                      p.CanWrite
                select p;

            foreach (var property in properties)
            {
                var value = (string)property.GetValue(obj, null);
                if (value == null)
                {
                    property.SetValue(obj, string.Empty, null);
                }
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return IsNullOrEmpty_(source);
        }

        private static bool IsNullOrEmpty_<T>(IEnumerable<T> source)
        {
            if (source == null || !source.Any())
                return true;
            return false;
        }
        
    }    

}