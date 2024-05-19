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



        public int? nameTopB { get; set; }
        public int? nameLeftB { get; set; }
        public int? accTopB { get; set; }
        public int? accLeftB { get; set; }

        public string? ref1 { get; set; }
        public string? ref2 { get; set; }
        public int? DateTopB { get; set; }
        public int? DateLeftB { get; set; }
        public int? wordsLeftB { get; set; }
        public int? wordsTopB { get; set; }
    }
}
