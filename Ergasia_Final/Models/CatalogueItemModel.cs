using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergasia_Final.Models
{
    public class CatalogueItemModel
    {
        public string Name { get; set; }
        public CatalogueItemTypes Type { get; set; }
        public double Price { get; set; }
        public bool AllergyWarning { get; set; } = false;
        public string? OptionalDescription { get; set; }
    }
}
