using System.Collections.Generic;

namespace Application.Common.Models
{
    /// <summary>
    /// Список с общим кол-вом
    /// </summary>
    public class TotalList<T>
    {
        /// <summary>
        /// Список объектов
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// Общее кол-во объектов удовлетряющих запросу
        /// </summary>
        public int Total { get; set; }

        public TotalList()
        {
        }

        public TotalList(List<T> items, int total)
        {
            Items = items;
            Total = total;
        }
    }
}