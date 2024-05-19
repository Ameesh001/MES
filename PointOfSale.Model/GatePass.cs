using System;
using System.Collections.Generic;

namespace PointOfSale.Model
{
    public partial class GatePass
    {
        public int idGatePass{ get; set; }
        public string? name { get; set; }
        public string? companyName { get; set; }
        public string? contactNo { get; set; }
        public string? nic { get; set; }
        public string? vechicleType { get; set; }
        public string? vechicleNo { get; set; }
		public string? Status { get; set; }
		public string? remarks { get; set; }
		public int? userID { get; set; }
        public DateTime? checkIn { get; set; }
        public DateTime? checkOut { get; set; }
        public string? item { get; set; }
		public string? itemDetail { get; set; }
        public DateTime? dateGP { get; set; }
        public int? isReceived { get; set; }

        //	public byte[]? Photo { get; set; }
    }
}
