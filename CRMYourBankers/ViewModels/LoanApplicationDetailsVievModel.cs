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
using System.Windows;

namespace CRMYourBankers.ViewModels
{
    public class LoanApplicationDetailsViewModel : TabBaseViewModel
    {
        public List<LoanApplication> LoanApplications { get; set; }
        public List<Bank> Banks { get; set; }
        public List<Client> Clients { get; set; }

        public int? AmountRequestedText { get; set; }
        public int? AmountReceivedText { get; set; }
        public int? ClientCommissionText { get; set; }
        public string TasksToDoText { get; set; }
        public int? BankId { get; set; }
        public int? ClientId { get; set; }
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
            SaveButtonCommand = new RelayCommand(() =>
            {
                if (SelectedloanApplication == null)
                {
                    var newLoanApplication = new LoanApplication
                    {
                        ClientId = ClientId ??0, //do pustej property z clasy LoanApplication wstaw wartość z property z LoanApplicationDetailsView
                                            //??0 oznacza, że ClientId z prawej będzie null to wstawi O
                        BankId = BankId ??0,
                        AmountRequested = AmountRequestedText,
                        AmountReceived = AmountReceivedText,
                        ClientCommission  = ClientCommissionText,
                        TasksToDo = TasksToDoText
                    };

                    if (!newLoanApplication.Validate())
                    {
                        MessageBox.Show("Niepoprawnie wypełnione dane lub puste pola",
                            "Błędy w formularzu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;//nic nie zwraca tylko kończy funkcje/metode SaveButtonCommand (void)
                    }

                    LoanApplications.Add(newLoanApplication);
                }
                else
                {
                    SelectedloanApplication.ClientId = ClientId ??0;
                    SelectedloanApplication.BankId = BankId ??0;
                    SelectedloanApplication.AmountRequested = AmountRequestedText;
                    SelectedloanApplication.AmountRequested = AmountRequestedText;
                    SelectedloanApplication.TasksToDo = TasksToDoText;

                    if (!SelectedloanApplication.Validate())
                    {
                        MessageBox.Show("Niepoprawnie wypełnione dane lub puste pola",
                            "Błędy w formularzu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;//nic nie zwraca tylko kończy funkcje/metode SaveButtonCommand (void)
                    }
                }

                MessageBox.Show($"Zapisano: {ClientId} {BankId} {AmountRequestedText}",
                    "Dodano Nowy Wniosek",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                TabMessenger.Send(new TabChangeMessage { TabName = "LoanApplicationSearch" });
                ClearAllFields();
            });
        }
        public void ClearAllFields()
        {
            ClientId = null;
            BankId = null;
            AmountRequestedText = null;
            AmountReceivedText = null;
            ClientCommissionText = null;
            TasksToDoText = null;
        }

    }
}

