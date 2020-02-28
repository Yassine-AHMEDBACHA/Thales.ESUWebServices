using System.Collections.Generic;

namespace ESU.Monitoring.Helpers
{
    public static class IIDictionaryExtensions
    {
        public static V GetOrAddValue<T,V>(this Dictionary<T, V> dictionary, T key) 
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
