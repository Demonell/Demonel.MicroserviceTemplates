using System;

namespace Application.Common.Models
{
    /// <summary>
    /// Диапазон дат
    /// </summary>
    public class DateRange
    {
        /// <summary>
        /// Начальная дата
        /// </summary>
        public DateTime? From { get; set; }
        
        /// <summary>
        /// Конечная дата
        /// </summary>
        public DateTime? To { get; set; }
    }
}
