using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Application.Common.Models
{
    public class Sortable
    {
        /// <summary>
        /// Сортировка
        /// <para>Формат: [+/-][fieldName]</para>
        /// <para>Первый символ обозначает направление сортировки</para> 
        /// <para>По возврастинаю "+"</para> 
        /// <para>По убыванию "-"</para> 
        /// <para>Несколько сортировок указываются через запятую</para>
        /// </summary>
        public string Sort { get; set; }

        [JsonIgnore]
        public List<Sort> Sorts => Sort?.Replace(" ", "").Split(',').Select(s => new Sort(s)).ToList() ?? new List<Sort>();
    }
}
