using System;
using System.Collections.Generic;

namespace PointOfSale.Model
{
    public partial class Colour
    {
        public Colour()
        {
            //Products = new HashSet<Product>();
        }

        public int IdCategory { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? RegistrationDate { get; set; }

        //public virtual ICollection<Product> Products { get; set; }
    }
}
