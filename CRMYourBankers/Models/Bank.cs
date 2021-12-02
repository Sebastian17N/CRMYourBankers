using System.Collections.Generic;

namespace CRMYourBankers.Models
{
    public class Bank
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public List<LoanApplication> LoanApplications { get; set; }
        public List<BankClientBIK> PersonalLoanClients { get; set; }        
    }
}
