using System.Collections.Generic;

namespace ESU.MonitoringWS.Extensions
{
    internal static class IIDictionaryExtensions
    {
        public static V GetOrAddValue<T,V>(this IDictionary<T, V> dictionary, T key) 
            where V : new()
        {
            if (!dictionary.TryGetValue(key, out var value))
            {
                dictionary[key] = value = new V();
            }

            return value;
        }
    }
}
