namespace CRMYourBankers.Models
{
    public class LoanApplication
    {        
        public int Id { get; set; }
        public int? AmountRequested { get; set; }
        public int? AmountReceived { get; set; }
        public int? ClientCommission { get; set; }
        public string TasksToDo { get; set; }        
        public int ClientId { get; set; }
        public int BankId { get; set; }
        
        public bool Validate()
        {
            return
                AmountRequested != null &&
                ClientId != 0 && // Jeśli ClientId lub BankId = 0 to oznacza, że nie wybrano ich z combo.
                BankId != 0;
        }
    }
}
