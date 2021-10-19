using CRMYourBankers.Database;
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
        public YourBankersContext Context { get; set; }
        public SummaryViewModel(Messenger messenger, YourBankersContext context) : base(messenger)
        {
            Context = context;
        }
    }
}
