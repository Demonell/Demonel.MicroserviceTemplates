using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> SmartOrderBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            if (enumerable.GetType() == typeof(IOrderedEnumerable<T>))
            {
                var orderedQuery = enumerable as IOrderedEnumerable<T>;
                return orderedQuery?.ThenBy(keySelector);
            }

            return enumerable.OrderBy(keySelector);
        }

        public static IEnumerable<T> SmartOrderByDescending<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            if (enumerable.GetType() == typeof(IOrderedEnumerable<T>))
            {
                var orderedQuery = enumerable as IOrderedEnumerable<T>;
                return orderedQuery?.ThenByDescending(keySelector);
            }

            return enumerable.OrderByDescending(keySelector);
        }
    }
}