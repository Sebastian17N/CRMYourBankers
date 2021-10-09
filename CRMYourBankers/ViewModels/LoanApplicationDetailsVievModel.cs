using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CRMYourBankers.Messages;

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
        private LoanApplication _selectedLoanApplication;
        public LoanApplication SelectedloanApplication
        {
            get => _selectedLoanApplication;
            set
            {
                _selectedLoanApplication = value;
                if (_selectedLoanApplication != null)
                {
                    AmountRequestedText = _selectedLoanApplication.AmountRequested;
                    AmountReceivedText = _selectedLoanApplication.AmountReceived;
                    ClientCommissionText = _selectedLoanApplication.ClientCommission;
                    TasksToDoText = _selectedLoanApplication.TasksToDo;
                }
                NotifyPropertyChanged("LoanApplications");
            }
        }

        public ICommand SaveButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand DetailsScreenOpenHandler { get; set; }

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
            DetailsScreenOpenHandler = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = "LoanApplicationDetails",
                    LoanApplication = SelectedloanApplication
                });
            });
        }


    }
}

