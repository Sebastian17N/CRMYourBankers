using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using System.Windows.Input;
using CRMYourBankers.Messages;
using System.Windows;
using CRMYourBankers.Database;
using System.Collections.ObjectModel;
using CRMYourBankers.ViewModels.Interfaces;
using System;
using CRMYourBankers.Enums;

namespace CRMYourBankers.ViewModels
{
    public class LoanApplicationDetailsViewModel : TabBaseViewModel, IRefreshReferenceDataOwner, 
        IClearAllFieldsOwner, IRefreshDataOwner, ILastTabNameOwner
    {
        #region UI property fields
        public int? AmountRequestedText { get; set; }
        public int? AmountReceivedText { get; set; }
        private string _clientCommissionText;
        public string ClientCommissionText 
        { 
            get=> _clientCommissionText;
            set
            {
                _clientCommissionText = value;
                NotifyPropertyChanged("CommissionGet");
            }
        }
        public string _brokerCommissionText;
        public string BrokerCommissionText 
        {
            get => _brokerCommissionText;
            set
            {
                _brokerCommissionText = value;
                NotifyPropertyChanged("CommissionGet");
            }
        }

        public int? CommissionGet => 
            ParseComission(ClientCommissionText) - ParseComission(BrokerCommissionText);

        public string TasksToDoText { get; set; }
        public int? BankId { get; set; }
        public int? ClientId { get; set; }
        public int? MultiBrokerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime LoanStartDate { get; set; }        
        public TabName LastTabName { get; set; }
        public object LastTabObject { get; set; }
        public LoanApplicationStatus? SelectedLoanApplicationStatus { get; set; }
        public bool IsPaid { get; set; }
        public string FullName { get; set; }
        #endregion
        
        public Client SelectedClient { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<Bank> Banks { get; set; }
        public ObservableCollection<MultiBroker> MultiBrokers { get; set; }

        private ObservableCollection<LoanTask> _loanTasks;
        public ObservableCollection<LoanTask> LoanTasks 
        { 
            get => _loanTasks; 
            set
			{
                if (value == null)
                    _loanTasks = new ObservableCollection<LoanTask>();
                else
				    _loanTasks = new ObservableCollection<LoanTask>(
					value
                        .OrderBy(task => task.Done)
                        .ThenByDescending(task => task.Id)
                        .ToList());
                NotifyPropertyChanged("LoanTask");
			}
        }

        private LoanApplication _selectedLoanApplication;
        public LoanApplication SelectedLoanApplication
        {
            get => _selectedLoanApplication;
            set
            {
                _selectedLoanApplication = value;
            }
        }

        public ICommand SaveButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AddNewLoanApplicationTaskButtonCommand { get; set; }
        public ICommand GoToSelectedClientButtonCommand { get; set; }
        public YourBankersContext Context { get; set; }

        public dynamic BankList { get; set; }

        public LoanApplicationDetailsViewModel(Messenger tabMessenger, YourBankersContext context) 
            : base(tabMessenger, TabName.LoanApplicationDetails)
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
                    SelectedLoanApplication = new LoanApplication
                    {
                        Id = Context.LoanApplications.Max(loan => loan.Id) + 1                        
                    };

                    Context.LoanApplications.Add(SelectedLoanApplication);
                }

                SelectedLoanApplication.ClientId = ClientId ?? 0;
                SelectedLoanApplication.BankId = BankId ?? 0;
                SelectedLoanApplication.AmountRequested = AmountRequestedText;
                SelectedLoanApplication.AmountRequested = AmountRequestedText;
                SelectedLoanApplication.ClientCommission = ParseComission(ClientCommissionText);
                SelectedLoanApplication.BrokerCommission = ParseComission(BrokerCommissionText);
                SelectedLoanApplication.StartDate = StartDate;
                SelectedLoanApplication.LoanStartDate = LoanStartDate;
                SelectedLoanApplication.Paid = IsPaid;
                SelectedLoanApplication.LoanApplicationStatus = SelectedLoanApplicationStatus;
                SelectedLoanApplication.MultiBrokerId = MultiBrokerId;
                SelectedLoanApplication.LoanTasks = LoanTasks;

                if (SelectedLoanApplication.Id == 0)
                {
                    Context.LoanApplications.Add(SelectedLoanApplication);
                    SelectedLoanApplication.Id = Context.LoanApplications.Max(loan => loan.Id) + 1;
                }

                if (!Validate() || !SelectedLoanApplication.Validate())
                {
                    MessageBox.Show("Niepoprawnie wypełnione dane lub puste pola",
                            "Błędy w formularzu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    return;
                }

                Context.SaveChanges();
                MessageBox.Show($"Zapisano: {SelectedLoanApplication.Client.FirstName} " +
                                  $"{SelectedLoanApplication.Client.LastName} " +
                                  $"{SelectedLoanApplication.Bank.Name} " +
                                  $"{AmountRequestedText}",
                    "Dodano Nowy Wniosek",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                TabMessenger.Send(new TabChangeMessage 
                { 
                    TabName = LastTabName,
                    SelectedObject = LastTabName == TabName.ClientDetails ? SelectedLoanApplication.Client : null
                });

                ClearAllFields();
            });
            
            //CancelButtonCommand = new RelayCommand(() =>
            //{
            //    ClearAllFields();
            //    TabMessenger.Send(new TabChangeMessage { TabName = LastTabName.ToString()});
            //});

            // Przykład napisania zwykłej funkcji, którą możesz zastąpić funkcją anonimową - patrz komentarz wyżej.
            CancelButtonCommand = new RelayCommand(CancelButtonCommandHandler);

            AddNewLoanApplicationTaskButtonCommand = new RelayCommand(() =>
            {
                if (SelectedLoanApplication != null)
                {
                    using (var context = new YourBankersContext())
                    {
                        var newLoanApplicationTask = new LoanTask
                        {
                            Text = TasksToDoText,
                            LoanApplicationId = SelectedLoanApplication.Id
                        };

                        context.LoanTasks.Add(newLoanApplicationTask);
                        context.SaveChanges();
                    }

                    SelectedLoanApplication.LoanTasks = new ObservableCollection<LoanTask>
                        (Context
                            .LoanTasks
                            .Where(task => task.LoanApplicationId == SelectedLoanApplication.Id)
                            .ToList());
                    LoanTasks = SelectedLoanApplication.LoanTasks;
                    NotifyPropertyChanged("LoanTasks");
                }

                TasksToDoText = string.Empty;
                NotifyPropertyChanged("TasksToDoText");

                MessageBox.Show($"Nowe zadanie dodane",
                    "Dodano Nowe Zadanie",
                   MessageBoxButton.OK,
                   MessageBoxImage.Information);
            });

            GoToSelectedClientButtonCommand = new RelayCommand(() =>
            {
                if (SelectedLoanApplication?.Client == null)
                    return;
                //? powoduje że jeżeli SelectedLoanApplication będzie puste to podstawi się null, a nie zawiesi program
                
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = TabName.ClientDetails,
                    SelectedObject = SelectedLoanApplication.Client,
                    LastTabName = TabName.LoanApplicationDetails,
                    LastTabObject = SelectedLoanApplication
                });
            });
        }

        private void CancelButtonCommandHandler ()
        {
            ClearAllFields();
            TabMessenger.Send(new TabChangeMessage 
            { 
                TabName = LastTabName,
                SelectedObject = LastTabName == TabName.ClientDetails ? SelectedLoanApplication.Client : null
            });
        }

        public void RefreshReferenceData()
        {
            Clients = new ObservableCollection<Client>(Context.Clients.ToList());
            //specjalny rodzaj listy, który który sam powiadamia widok, że się zmienił bez potrzeby 
            //wywoływania NotifyPropertyChanged
            Banks = new ObservableCollection<Bank>(Context.Banks.ToList());
            MultiBrokers = new ObservableCollection<MultiBroker>(Context.MultiBrokers.ToList());
        }

        public void ClearAllFields()
        {
            ClientId = null;
            BankId = null;
            AmountRequestedText = null;
            AmountReceivedText = null;
            ClientCommissionText = null;
            BrokerCommissionText = null;
            TasksToDoText = null;
            StartDate = DateTime.Now;
            LoanStartDate = DateTime.Now;
            IsPaid = false;
            MultiBrokerId = null;
            LoanTasks = null;
            SelectedLoanApplicationStatus = LoanApplicationStatus.Submited;
        }

        public void RefreshData()
        {
            if (SelectedLoanApplication != null)
            {
                BankId = _selectedLoanApplication.BankId;
                ClientId = _selectedLoanApplication.ClientId;
                AmountRequestedText = _selectedLoanApplication.AmountRequested;
                AmountReceivedText = _selectedLoanApplication.AmountReceived;
                ClientCommissionText = _selectedLoanApplication.ClientCommission?.ToString();
                BrokerCommissionText = _selectedLoanApplication.BrokerCommission?.ToString();
                StartDate = _selectedLoanApplication.StartDate;
                LoanStartDate = _selectedLoanApplication.LoanStartDate;
                IsPaid = _selectedLoanApplication.Paid;
                SelectedLoanApplicationStatus = _selectedLoanApplication.LoanApplicationStatus;
                MultiBrokerId = _selectedLoanApplication.MultiBrokerId;
                LoanTasks = _selectedLoanApplication.LoanTasks;
            }
            NotifyPropertyChanged("LoanApplication");
        }

        public int? ParseComission(string comission)
		{
            var converted = int.TryParse(comission, out var convertedValue);
            return converted ? convertedValue : null;
        }

        public bool Validate()
		{
            return
                string.IsNullOrEmpty(ClientCommissionText) || (int.TryParse(ClientCommissionText, out var clientValue) && clientValue >= 0) 
                && string.IsNullOrEmpty(BrokerCommissionText) || (int.TryParse(BrokerCommissionText, out var brokerValue) && brokerValue >= 0)
                && clientValue >= brokerValue;
		}
    }
}

