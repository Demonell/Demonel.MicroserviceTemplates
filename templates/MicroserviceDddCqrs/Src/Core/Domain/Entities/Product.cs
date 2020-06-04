using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Product : Auditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Material> Materials { get; set; }
        public ProductType ProductType { get; set; }
    }
}
