using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Linq;
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
        
        #region UI property fields
        public int? AmountRequestedText { get; set; }
        public int? AmountReceivedText { get; set; }
        public int? ClientCommissionText { get; set; }
        public string TasksToDoText { get; set; }
        public int? BankId { get; set; }
        public int? ClientId { get; set; }
        #endregion

        private LoanApplication _selectedLoanApplication;
        public LoanApplication SelectedLoanApplication
        {
            get => _selectedLoanApplication;
            set
            {
                _selectedLoanApplication = value;
                if (_selectedLoanApplication != null)
                {
                    BankId = _selectedLoanApplication.BankId;
                    ClientId = _selectedLoanApplication.ClientId;
                    AmountRequestedText = _selectedLoanApplication.AmountRequested;
                    AmountReceivedText = _selectedLoanApplication.AmountReceived;
                    ClientCommissionText = _selectedLoanApplication.ClientCommission;
                    TasksToDoText = _selectedLoanApplication.TasksToDo;
                }
                else
                {
                    ClearAllFields();
                }

                NotifyPropertyChanged("LoanApplication");
            }
        }

        private List<LoanTask> _loanTasks;
        public List<LoanTask> LoanTasks 
        { 
            get => _loanTasks;
            set
            {
                _loanTasks = value;                
                NotifyPropertyChanged("LoanTasks");
            }
        }
        private LoanTask _selectedLoanTasks;
        public LoanTask SelectedLoanTasks
        {
            get => _selectedLoanTasks;
            set
            {
                _selectedLoanTasks = value;
                if (_selectedLoanTasks != null)
                {
                    TasksToDoText = _selectedLoanTasks.Text;
                }
                NotifyPropertyChanged("LoanTask");
            }
            
        }
        public ICommand SaveButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }        

        public LoanApplicationDetailsViewModel(Messenger tabMessenger, 
            List<LoanApplication> loanApplications, List<Client> clients, List<Bank> banks, List<LoanTask> loanTasks) 
            : base(tabMessenger)
        {
            LoanApplications = loanApplications;
            Clients = clients;
            Banks = banks;
            LoanTasks = loanTasks;

            RegisterCommands();
        }

        public void RegisterCommands()
        {
            SaveButtonCommand = new RelayCommand(() =>
            {
                if (SelectedLoanApplication == null)
                {
                    var newLoanApplication = new LoanApplication
                    {
                        Id = LoanApplications.Max(loan => loan.Id) + 1,
                        ClientId = ClientId ?? 0, //do pustej property z clasy LoanApplication wstaw wartość z property z LoanApplicationDetailsView
                                            //??0 oznacza, że ClientId z prawej będzie null to wstawi O
                        BankId = BankId ?? 0,
                        AmountRequested = AmountRequestedText,
                        AmountReceived = AmountReceivedText,
                        ClientCommission  = ClientCommissionText,                       
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
                    SelectedLoanApplication.ClientId = ClientId ??0;
                    SelectedLoanApplication.BankId = BankId ??0;
                    SelectedLoanApplication.AmountRequested = AmountRequestedText;
                    SelectedLoanApplication.AmountRequested = AmountRequestedText;                    

                    if (!SelectedLoanApplication.Validate())
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
            CancelButtonCommand = new RelayCommand(() =>
            {
                ClearAllFields();
                TabMessenger.Send(new TabChangeMessage { TabName = "LoanApplicationSearch" });
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

            SelectedLoanApplication = null;
        }
    }
}

