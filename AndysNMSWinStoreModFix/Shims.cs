using System.Collections.Generic;

namespace AndysNMSWinStoreModFix
{
    internal static class Shims
    {
        internal static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }
    }
}