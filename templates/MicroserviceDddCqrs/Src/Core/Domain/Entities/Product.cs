using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Product : Auditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductType ProductType { get; set; }
        public DateTimeOffset DeliveryDate { get; set; }
        public List<Material> Materials { get; set; }
    }
}
