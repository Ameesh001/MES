using System;
using System.Collections.Generic;

namespace PointOfSale.Model
{
    public partial class Cheque
    {
        public DateTime? sysdate { get; set; }
        public int idcheque { get; set; }
        public string? payeeName { get; set; }
        public string? amount { get; set; }
        public string? amountInWords { get; set; }
        public string? chequeNo { get; set; }
        public DateTime? depositDate { get; set; }
        public DateTime? editDate { get; set; }
        public int? bank { get; set; }
        public string? States { get; set; }
		public string? nameTop { get; set; }
		public string? nameLeft { get; set; }
        public string? accTop { get; set; }
		public string? accLeft { get; set; }

        public string? CheqNoTop { get; set; }
        public string? CheqNoLeft { get; set; }
        public string? DateTop { get; set; }
        public string? DateLeft { get; set; }
        public string? wordsLeft { get; set; }
        public string? wordsTop { get; set; }
        public string? userID { get; set; }
        public string? chequeType { get; set; }

        //	public byte[]? Photo { get; set; }
    }
}
