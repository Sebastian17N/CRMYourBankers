﻿using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CRMYourBankers.ViewModels
{
    public class LoanApplicationDetailsViewModel : TabBaseViewModel
    {
        public List<LoanApplication> LoanApplications { get; set; }
        public List<Bank> Banks { get; set; }
        public List<Client> Clients { get; set; }

        public int AmountRequestedText { get; set; }
        public int AmountReceivedText { get; set; }
        public int ClientCommissionText { get; set; }
        public string TasksToDoText { get; set; }

        public ICommand SaveButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }

        public LoanApplicationDetailsViewModel(Messenger tabMessenger, List<LoanApplication> loanApplications, List<Client> clients, List<Bank> banks) 
            : base(tabMessenger)
        {
            LoanApplications = loanApplications;
            Clients = clients;
            Banks = banks;
            RegisterCommands();
        }

        public void RegisterCommands()
        {

        }


    }
}

