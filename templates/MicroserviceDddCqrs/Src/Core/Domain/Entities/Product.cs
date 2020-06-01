using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Product : AuditableEntity<int>
    {
        public string Name { get; set; }
        public List<Material> Materials { get; set; }
        public ProductType ProductType { get; set; }
    }
}
