namespace ShaantInfosystems.Web.Models
{
    public class NseModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long SharesTraded { get; set; }
        public decimal TurnOverInRsCore { get; set; }
        public int DataType { get; set; }
    }
}
