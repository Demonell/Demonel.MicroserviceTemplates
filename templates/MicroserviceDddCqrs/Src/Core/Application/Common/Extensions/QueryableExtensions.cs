using System;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> SmartOrderBy<T, TKey>(this IQueryable<T> queryable, Expression<Func<T, TKey>> keySelector)
        {
            if (queryable.Expression.Type == typeof(IOrderedQueryable<T>))
            {
                var orderedQuery = queryable as IOrderedQueryable<T>;
                return orderedQuery?.ThenBy(keySelector);
            }

            return queryable.OrderBy(keySelector);
        }

        public static IQueryable<T> SmartOrderByDescending<T, TKey>(this IQueryable<T> queryable, Expression<Func<T, TKey>> keySelector)
        {
            if (queryable.Expression.Type == typeof(IOrderedQueryable<T>))
            {
                var orderedQuery = queryable as IOrderedQueryable<T>;
                return orderedQuery?.ThenByDescending(keySelector);
            }

            return queryable.OrderByDescending(keySelector);
        }
    }
}