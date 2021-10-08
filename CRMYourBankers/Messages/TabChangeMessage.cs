using CRMYourBankers.Models;

namespace CRMYourBankers.Messages
{
    public class TabChangeMessage
    {
        public string TabName { get; set; }
        public Client Client { get; set; }
        public LoanApplication LoanApplication { get; set; }
        public Bank Bank { get; set; }
    }
}
