namespace CRMYourBankers.Models
{
    public class BankClientPersonalLoan
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int BankId { get; set; }
        public Bank Bank { get; set; }

        // Krok pierwszy
        //public BikLoanType BikLoanType { get; set; }

        // Krok drugi
        //public int LoanIndex { get; set; }
    }
}
