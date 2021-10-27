using System;

namespace CRMYourBankers.Models
{
    public class MonthSummary
    {
        public int Id { get; set; }
        public DateTime Month { get; set; }
        public int EstimatedTarget { get; set; }
        public DateTime PresentMonth { get; set; } = DateTime.Now;
        public string MonthDisplay 
            => Month.ToString("yyyy/MM");
        public bool Validate()
        {
            return
            EstimatedTarget != null;
        }
    }
}
