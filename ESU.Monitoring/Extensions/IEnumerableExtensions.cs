using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.MonitoringWS.Extensions
{
    internal static class IEnumerableExtensions
    {
        internal static SortedDictionary<T, V> ToSortedDictionary<T, V, U>(this IEnumerable<U> enumerable, Func<U, T> keySelector, Func<U, V> ValueSelector)
        {
            var dictionnary = new SortedDictionary<T, V>();
            foreach (var item in enumerable)
            {
                dictionnary.Add(keySelector(item), ValueSelector(item));
            }

            return dictionnary;
        }

        internal static SortedDictionary<T, V> ToSortedDictionary<T, V>(this IEnumerable<V> enumerable, Func<V, T> keySelector)
        {
            var dictionnary = new SortedDictionary<T, V>();
            foreach (var item in enumerable)
            {
                dictionnary.Add(keySelector(item), item);
            }

            return dictionnary;
        }
    }
}
