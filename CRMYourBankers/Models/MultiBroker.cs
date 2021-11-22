using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMYourBankers.Models
{
    public class MultiBroker
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<LoanApplication> LoanApplications { get; set; }
    }
}
