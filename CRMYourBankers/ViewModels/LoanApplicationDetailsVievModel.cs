using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CRMYourBankers.Messages;
using System.Windows;
using CRMYourBankers.Database;
using System.Collections.ObjectModel;
using CRMYourBankers.ViewModels.Interfaces;
using System;

namespace CRMYourBankers.ViewModels
{
    public class LoanApplicationDetailsViewModel : TabBaseViewModel, IRefreshReferenceDataOwner, 
        IClearAllFieldsOwner, IRefreshDataOwner
    {
        #region UI property fields
        public int? AmountRequestedText { get; set; }
        public int? AmountReceivedText { get; set; }
        public int? ClientCommissionText { get; set; }
        public string TasksToDoText { get; set; }
        public int? BankId { get; set; }
        public int? ClientId { get; set; }
        public DateTime LoanStartDate { get; set; }
        #endregion

        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<Bank> Banks { get; set; }
        
        private LoanApplication _selectedLoanApplication;
        public LoanApplication SelectedLoanApplication
        {
            get => _selectedLoanApplication;
            set
            {
                _selectedLoanApplication = value;
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

        public ICommand SaveButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public YourBankersContext Context { get; set; }
        public dynamic BankList { get; set; }

        public LoanApplicationDetailsViewModel(Messenger tabMessenger, YourBankersContext context) 
            : base(tabMessenger)
        {
            Context = context;
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
                        Id = Context.LoanApplications.Max(loan => loan.Id) + 1,
                        ClientId = ClientId ??0, //do pustej property z clasy LoanApplication wstaw wartość z property z LoanApplicationDetailsView
                                            //??0 oznacza, że ClientId z prawej będzie null to wstawi O
                        BankId = BankId ??0,
                        AmountRequested = AmountRequestedText,
                        AmountReceived = AmountReceivedText,
                        ClientCommission  = ClientCommissionText,
                        LoanStartDate = LoanStartDate                
                    };

                    if (!newLoanApplication.Validate())
                    {
                        MessageBox.Show("Niepoprawnie wypełnione dane lub puste pola",
                            "Błędy w formularzu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;//nic nie zwraca tylko kończy funkcje/metode SaveButtonCommand (void)
                    }

                    Context.LoanApplications.Add(newLoanApplication);
                }
                else
                {
                    SelectedLoanApplication.ClientId = ClientId ?? 0;
                    SelectedLoanApplication.BankId = BankId ?? 0;
                    SelectedLoanApplication.AmountRequested = AmountRequestedText;
                    SelectedLoanApplication.AmountRequested = AmountRequestedText;
                    SelectedLoanApplication.LoanStartDate = LoanStartDate;

                    if (!SelectedLoanApplication.Validate())
                    {
                        MessageBox.Show("Niepoprawnie wypełnione dane lub puste pola",
                            "Błędy w formularzu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;//nic nie zwraca tylko kończy funkcje/metode SaveButtonCommand (void)
                    }
                }

                Context.SaveChanges();
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

        public void RefreshReferenceData()
        {
            Clients = new ObservableCollection<Client>(Context.Clients.ToList());
            //specjalny rodzaj listy, który który sam powiadamia widok, że się zmienił bez potrzeby 
            //wywoływania NotifyPropertyChanged
            Banks = new ObservableCollection<Bank>(Context.Banks.ToList());
        }

        public void ClearAllFields()
        {
            ClientId = null;
            BankId = null;
            AmountRequestedText = null;
            AmountReceivedText = null;
            ClientCommissionText = null;
            TasksToDoText = null;
            LoanStartDate = DateTime.MinValue;   
        }

        public void RefreshData()
        {
            if (SelectedLoanApplication != null)
            {
                BankId = _selectedLoanApplication.BankId;
                ClientId = _selectedLoanApplication.ClientId;
                AmountRequestedText = _selectedLoanApplication.AmountRequested;
                AmountReceivedText = _selectedLoanApplication.AmountReceived;
                ClientCommissionText = _selectedLoanApplication.ClientCommission;
                TasksToDoText = _selectedLoanApplication.TasksToDo;
                LoanStartDate = _selectedLoanApplication.LoanStartDate;
            }
            NotifyPropertyChanged("LoanApplication");
        }
    }
}

