using System;
using System.Collections.Generic;

namespace PointOfSale.Model
{
    public partial class Bank
    {
        public Bank()
        {
			Customers = new HashSet<Customer>();
        }

        public int IdCategory { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
