using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMYourBankers.ViewModels
{
    public class SummaryViewModel : TabBaseViewModel
    {
        public List<Client> Clients { get; set; }
        public List<LoanApplication> LoanApplications { get; set; }
        public List<Bank> Banks { get; set; }
        public SummaryViewModel(Messenger messenger, 
            List<LoanApplication> loanApplications, List<Bank> banks, List<Client> clients) : base(messenger)
        {
            Clients = clients;
            LoanApplications = loanApplications;
            Banks = banks;
        }
    }
}
