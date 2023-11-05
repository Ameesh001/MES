namespace PointOfSale.Models
{
    public class VMCustomer
    {
        public int IdProduct { get; set; }
        public string? CusCode { get; set; }
        public string? invoiceName { get; set; }
        public string? ShortName { get; set; }
        public int? IdBank { get; set; }
        public string? NameCategory { get; set; }
        public string? PhoneNo { get; set; }
        public string? Mobile { get; set; }
		public string? Address { get; set; }
		public string? OpeningBalance { get; set; }
		public string? Debit { get; set; }
		public byte[]? Photo { get; set; }
        public string? PhotoBase64 { get; set; }
        public int? IsActive { get; set; }

		public string? RegistrationDate { get; set; }
		//public DateTime? RegistrationDate { get; set; }
	}
}
