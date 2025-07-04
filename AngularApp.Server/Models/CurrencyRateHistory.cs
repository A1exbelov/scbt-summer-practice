namespace AngularApp.Server.Models
{
    public class CurrencyRateHistory
    {
        public int Id { get; set; }
        public string CurrencyName { get; set; }
        public double Rate { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}