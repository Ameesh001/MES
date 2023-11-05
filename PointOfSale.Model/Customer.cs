using System;
using System.Collections.Generic;

namespace PointOfSale.Model
{
    public partial class Customer
    {
        public int IdProduct{ get; set; }
        public string? CusCode { get; set; }
        public string? invoiceName { get; set; }
        public string? ShortName { get; set; }
        public int? IdBank { get; set; }
        public string? PhoneNo { get; set; }
        public string? Mobile { get; set; }
		public string? OpeningBalance { get; set; }
		public string? Debit { get; set; }
		public string? Address { get; set; }
		public byte[]? Photo { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? RegistrationDate { get; set; }
		public virtual Bank? IdCategoryNavigation { get; set; }
	}
}
