using Application.Common.Interfaces;

namespace Application.Common.Models
{
    public class SortablePageable : Sortable, IPageable
    {
        /// <summary>
        /// Кол-во записей, которые необходимо пропустить
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        /// Максимальное кол-во возвращаемых записей
        /// </summary>
        public int? Take { get; set; }
    }
}
