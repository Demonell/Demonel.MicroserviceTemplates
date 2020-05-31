using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Application.Common.Extensions;

namespace Application.Common.Models
{
    public class Sort
    {
        public Sort()
        {
        }

        public Sort(string sortString)
        {
            Field = sortString[1..];
            Descending = sortString[0] == '-';
        }

        public string Field { get; set; }
        public bool Descending { get; set; }

        public IQueryable<T> Order<T, TKey>(IQueryable<T> query, Expression<Func<T, TKey>> keySelector)
        {
            return Descending
                ? query.SmartOrderByDescending(keySelector)
                : query.SmartOrderBy(keySelector);
        }

        public IOrderedEnumerable<T> Order<T, TKey>(List<T> query, Func<T, TKey> keySelector)
        {
            return Descending
                ? query.OrderByDescending(keySelector)
                : query.OrderBy(keySelector);
        }
    }
}