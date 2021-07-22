using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tree.Utilities.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> SelectRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector) where T : class
        {
            foreach (var parent in source)
            {
                yield return parent;

                var children = selector(parent);

                foreach (var child in SelectRecursive(children, selector))
                {
                    yield return child;
                }
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) where T :class
        {
            return (source == null || !source.Any());
        }
    }
}
