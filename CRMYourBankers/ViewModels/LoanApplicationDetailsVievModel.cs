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
using CRMYourBankers.Models.Interfaces;

namespace CRMYourBankers.ViewModels
{
    public class LoanApplicationDetailsViewModel : TabBaseViewModel, 
        IRefreshReferenceDataOwner, IClearAllFieldsOwner, IRefreshDataOwner, 
        ILastTabNameOwner, ISelectedItemOwner<LoanApplication>
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
        public DateTime? LoanStartDate { get; set; }        
        public TabName LastTabName { get; set; }
        public IEditable LastTabObject { get; set; }
        private LoanApplicationStatus? _selectedLoanApplicationStatus;
        public LoanApplicationStatus? SelectedLoanApplicationStatus 
        { 
            get => _selectedLoanApplicationStatus;
            set
            {
                var originalLoanApplicationStatus = _selectedLoanApplicationStatus;
                _selectedLoanApplicationStatus = value;

                if (!_isClearingData &&
                    SelectedLoanApplicationStatus != LoanApplicationStatus.Launched &&
                    originalLoanApplicationStatus == LoanApplicationStatus.Launched)
                {
                    LoanStartDate = null;
                    NotifyPropertyChanged("LoanStartDate");

                    MessageBox.Show("UWAGA! zmiana statusu uruchomionego kredytu powoduje usunięcie daty uruchomienia",
                            "Zmiany w formularzu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                }
            }            
         }        
        public bool IsPaid { get; set; }
        private bool _isClearingData = false;
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

        private LoanApplication _selectedItem;
        public LoanApplication SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
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
                if (SelectedItem == null)
                {
                    SelectedItem = new LoanApplication
                    {
                        Id = Context.LoanApplications.Max(loan => loan.Id) + 1                        
                    };

                    Context.LoanApplications.Add(SelectedItem);
                }

                SelectedItem.ClientId = ClientId ?? 0;
                SelectedItem.BankId = BankId ?? 0;
                SelectedItem.AmountRequested = AmountRequestedText;
                SelectedItem.AmountRequested = AmountRequestedText;
                SelectedItem.ClientCommission = ParseComission(ClientCommissionText);
                SelectedItem.BrokerCommission = ParseComission(BrokerCommissionText);
                SelectedItem.StartDate = StartDate;
                SelectedItem.LoanStartDate = LoanStartDate;
                SelectedItem.Paid = IsPaid;
                SelectedItem.LoanApplicationStatus = SelectedLoanApplicationStatus;
                SelectedItem.MultiBrokerId = MultiBrokerId;
                SelectedItem.LoanTasks = LoanTasks;

                if (SelectedItem.Id == 0)
                {
                    Context.LoanApplications.Add(SelectedItem);
                    SelectedItem.Id = Context.LoanApplications.Max(loan => loan.Id) + 1;
                }

                if (!Validate() || !SelectedItem.Validate())
                {
                    MessageBox.Show("Niepoprawnie wypełnione dane lub puste pola",
                            "Błędy w formularzu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    return;
                }

                //if (originalLoanApplicationStatus == ClientStatus.Active && SelectedItem.ClientStatus != ClientStatus.Active
                //    && SelectedItem.ClientTasks.Any(task => task.TaskDate <= System.DateTime.Now && !task.Done))
                //{
                //    MessageBox.Show("Zmiana statusu Klienta niemożliwa, ponieważ istnieją zaległe zadania.",
                //            "Błędy w formularzu",
                //            MessageBoxButton.OK,
                //            MessageBoxImage.Warning);
                //}



                Context.SaveChanges();
                MessageBox.Show($"Zapisano: {SelectedItem.Client.FirstName} " +
                                  $"{SelectedItem.Client.LastName} " +
                                  $"{SelectedItem.Bank.Name} " +
                                  $"{AmountRequestedText}",
                    "Dodano Nowy Wniosek",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                TabMessenger.Send(new TabChangeMessage 
                { 
                    TabName = LastTabName,
                    SelectedObject = LastTabName == TabName.ClientDetails ? SelectedItem.Client : null,
                    GoFurther = false
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
                if (SelectedItem != null)
                {
                    using (var context = new YourBankersContext())
                    {
                        var newLoanApplicationTask = new LoanTask
                        {
                            Text = TasksToDoText,
                            LoanApplicationId = SelectedItem.Id
                        };

                        context.LoanTasks.Add(newLoanApplicationTask);
                        context.SaveChanges();
                    }

                    SelectedItem.LoanTasks = new ObservableCollection<LoanTask>
                        (Context
                            .LoanTasks
                            .Where(task => task.LoanApplicationId == SelectedItem.Id)
                            .ToList());
                    LoanTasks = SelectedItem.LoanTasks;
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
                if (SelectedItem?.Client == null)
                    return;
                //? powoduje że jeżeli SelectedLoanApplication będzie puste to podstawi się null, a nie zawiesi program
                
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = TabName.ClientDetails,
                    SelectedObject = SelectedItem.Client,
                    LastTabName = TabName.LoanApplicationDetails,
                    LastTabObject = SelectedItem
                });
            });
        }

        private void CancelButtonCommandHandler ()
        {
            ClearAllFields();
            TabMessenger.Send(new TabChangeMessage 
            { 
                TabName = LastTabName,
                SelectedObject = LastTabObject,
                GoFurther = false
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
            _isClearingData = true;
            ClientId = null;
            BankId = null;
            AmountRequestedText = null;
            AmountReceivedText = null;
            ClientCommissionText = null;
            BrokerCommissionText = null;
            TasksToDoText = null;
            StartDate = DateTime.Now;
            LoanStartDate = null;
            IsPaid = false;
            MultiBrokerId = null;
            LoanTasks = null;
            SelectedLoanApplicationStatus = LoanApplicationStatus.Submited;
            _isClearingData = false;
        }

        public void RefreshData()
        {
            if (SelectedItem != null)
            {
                BankId = _selectedItem.BankId;
                ClientId = _selectedItem.ClientId;
                AmountRequestedText = _selectedItem.AmountRequested;
                AmountReceivedText = _selectedItem.AmountReceived;
                ClientCommissionText = _selectedItem.ClientCommission?.ToString();
                BrokerCommissionText = _selectedItem.BrokerCommission?.ToString();
                StartDate = _selectedItem.StartDate;
                LoanStartDate = _selectedItem.LoanStartDate;
                IsPaid = _selectedItem.Paid;
                SelectedLoanApplicationStatus = _selectedItem.LoanApplicationStatus;
                MultiBrokerId = _selectedItem.MultiBrokerId;
                LoanTasks = _selectedItem.LoanTasks;
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

