namespace CRMYourBankers.Models
{
    public class LoanTask
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Done { get; set; }
        public int LoanApplicationId { get; set; }
       // public LoanApplication LoanApplication { get; set; }

    }
}
