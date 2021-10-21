using System;

namespace CRMYourBankers.Models
{
    public class MonthSummary
    {
        public int Id { get; set; }
        public DateTime Month { get; set; }
        public int EstimatedTarget { get; set; }

        public string MonthDisplay 
            => Month.ToString("yyyy/MM");
    }
}
