using System;

namespace Application.Common.Models
{
    /// <summary>
    /// Диапазон дат
    /// </summary>
    public class DateTimeOffsetRange
    {
        /// <summary>
        /// Начальная дата
        /// </summary>
        public DateTimeOffset? From { get; set; }
        
        /// <summary>
        /// Конечная дата
        /// </summary>
        public DateTimeOffset? To { get; set; }
    }
}
