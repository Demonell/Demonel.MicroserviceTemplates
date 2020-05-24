using System;
using System.Collections.Generic;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities
{
    [Owned]
    public class Material : ValueObject
    {
        public string Name { get; private set; }
        public TimeSpan Durability { get; private set; }

        private Material() { }

        public Material(string name, TimeSpan durability)
        {
            Name = name;
            Durability = durability;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            // Using a yield return statement to return each element one at a time
            yield return Name;
            yield return Durability;
        }
    }
}
