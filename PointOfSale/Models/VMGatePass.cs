namespace PointOfSale.Models
{
    public class VMGatePass
	{
		public int idGatePass { get; set; }
		public string? name { get; set; }
		public string? companyName { get; set; }
		public string? contactNo { get; set; }
		public string? nic { get; set; }
		public string? vechicleType { get; set; }
		public string? vechicleNo { get; set; }
		public string? Status { get; set; }
		public string? remarks { get; set; }
		public int? userID { get; set; }
		public string? item { get; set; }
		public string? itemDetail { get; set; }
        public DateTime? checkIn { get; set; }
        public DateTime? checkOut { get; set; }
        public DateTime? dateGP { get; set; }
        public int? isReceived { get; set; }
        //public int IdProduct { get; set; }
        //      public string? CusCode { get; set; }
        //      public string? invoiceName { get; set; }
        //      public string? ShortName { get; set; }
        //      public int? IdBank { get; set; }
        //      public string? NameCategory { get; set; }
        //      public string? PhoneNo { get; set; }
        //      public string? Mobile { get; set; }
        //public string? Address { get; set; }
        //public string? OpeningBalance { get; set; }
        //public string? Debit { get; set; }
        //	public byte[]? Photo { get; set; }
        //      public string? PhotoBase64 { get; set; }
        //      public int? IsActive { get; set; }

        //public string? RegistrationDate { get; set; }
        //public DateTime? RegistrationDate { get; set; }
    }
}
