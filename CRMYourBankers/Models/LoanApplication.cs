using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMYourBankers.Models
{
    public class LoanApplication
    {        
        public int AmountRequested { get; set; }
        public int AmountReceived { get; set; }
        public int ClientCommission { get; set; }
        public string TasksToDo { get; set; }

        public int ClientId { get; set; }
        public int BankId { get; set; }
    }
}
