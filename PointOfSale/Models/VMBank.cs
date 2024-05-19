namespace PointOfSale.Models
{
    public class VMBank
    {
        public int IdCategory { get; set; }
        public string? Description { get; set; }
        public int? IsActive { get; set; }

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
