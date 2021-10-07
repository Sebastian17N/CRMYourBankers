using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMYourBankers.ViewModels
{
    class LoanApplicationDetailsVievModel : TabBaseViewModel
    {
        public List<LoanApplication> LoanApplications { get; set; }
        public List<Bank> Banks { get; set; }

    }
}

