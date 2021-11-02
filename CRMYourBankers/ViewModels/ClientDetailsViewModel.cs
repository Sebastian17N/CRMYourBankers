using CRMYourBankers.Database;
using CRMYourBankers.Messages;
using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using CRMYourBankers.ViewModels.Interfaces;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CRMYourBankers.Enum;


namespace CRMYourBankers.ViewModels
{
    public class ClientDetailsViewModel : TabBaseViewModel, IRefreshDataOwner
    {
        private Client _selectedClients;
        public Client SelectedClient 
        { 
            get => _selectedClients; 
            set
            {
                _selectedClients = value;

                if (_selectedClients != null)
                {
                    FirstNameText = _selectedClients.FirstName;
                    LastNameText = _selectedClients.LastName;
                    EmailText = _selectedClients.Email;
                    PhoneNumberText = _selectedClients.PhoneNumber;
                    PersonalIdText = _selectedClients.PersonalId;
                }
                else
                {
                    ClearAllFields();
                }
            }
        }

        //public bool EditingClient => SelectedClient != null;

        //public bool EditingClinetFunction()
        //{
        //    return SelectedClient != null;
        //}

        // Wydłużony zapis, tego co na dole dla FirstNameText
        //private string _firstNameText2;
        //public string FirstNameText2 
        //{
        //    get => _firstNameText2;
        //    set
        //    {
        //        _firstNameText2 = value;
        //    }
        //}

        public string FirstNameText { get; set; }
        public string LastNameText { get; set; }
        public string EmailText { get; set; }
        public int? PhoneNumberText { get; set; }
        public long? PersonalIdText { get; set; }

        public dynamic LoanApplicationsForClient { get; set; }
        private List<ClientTask> _clientTasks;
        public List<ClientTask> ClientTasks
        {
            get => _clientTasks;
            set 
            { 
                _clientTasks = value;
                NotifyPropertyChanged("ClientTask");
            }
        }        
        public ICommand SaveButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public YourBankersContext Context { get; set; }
        public ZusUs ZusUs { get; set; }

        public ClientDetailsViewModel(Messenger tabMessenger, YourBankersContext context)
            : base(tabMessenger, TabName.ClientDetails)
        {
            Context = context;
            RegisterCommands();
        }

        public void RefreshData()
        {
            if (SelectedClient == null)
            {
                return;
            }
            LoanApplicationsForClient =
                Context.LoanApplications
                    .Include(loan=>loan.LoanTasks)
                    .Where(loan => loan.ClientId == SelectedClient.Id) 
                    .Join(
                    Context.Banks,
                    loan => loan.BankId,
                    bank => bank.Id,
                    (loan, bank) => new
                    {
                        loan.ClientId,
                        loan.AmountRequested,
                        loan.TasksToDo,
                        BankName = bank.Name
                    })
                .Join(
                    Context.Clients,
                    loan => loan.ClientId,
                    client => client.Id,
                    (loan, client) => new
                    {
                        loan.BankName,
                        loan.AmountRequested,
                        loan.TasksToDo
                    }).ToList(); 
        }
        
        public void RegisterCommands()
        {
            SaveButtonCommand = new RelayCommand(() =>
            {
                if (SelectedClient == null)
                {
                    var newClient = new Client
                    {
                        Id = Context.Clients.Max(client => client.Id) + 1,
                        FirstName = FirstNameText,
                        LastName = LastNameText,
                        PhoneNumber = PhoneNumberText,
                        Email = EmailText,
                        PersonalId = PersonalIdText
                    };

                    if (!newClient.Validate())
                    {
                        MessageBox.Show("Niepoprawnie wypełnione dane lub puste pola",
                            "Błędy w formularzu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;//nic nie zwraca tylko kończy funkcje/metode SaveButtonCommand (void)
                    }

                    if (Context.Clients.Any(item => item.PersonalId == PersonalIdText))
                    {
                        MessageBox.Show("Klient o podanym nr Pesel już istnieje",
                            "Błędy w formularzu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }

                    Context.Clients.Add(newClient);                    
                }
                else
                {
                    SelectedClient.FirstName = FirstNameText;
                    SelectedClient.LastName = LastNameText;
                    SelectedClient.PhoneNumber = PhoneNumberText;
                    SelectedClient.Email = EmailText;
                    SelectedClient.PersonalId = PersonalIdText;

                    if (!SelectedClient.Validate())
                    {
                        MessageBox.Show("Niepoprawnie wypełnione dane lub puste pola",
                            "Błędy w formularzu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;//nic nie zwraca tylko kończy funkcje/metode SaveButtonCommand (void)
                    }
                }
                Context.SaveChanges();

                MessageBox.Show($"Zapisano: {FirstNameText} {LastNameText}", 
                    "Dodano Klienta",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                TabMessenger.Send(new TabChangeMessage { TabName = "ClientSearch" });
                ClearAllFields();                              
            });

            CancelButtonCommand = new RelayCommand(() =>
            {
                ClearAllFields();
                TabMessenger.Send(new TabChangeMessage { TabName = "ClientSearch" });
            });
        }

        public void ClearAllFields()
        {
            FirstNameText = "";
            LastNameText = "";
            PhoneNumberText = null;
            EmailText = "";
            PersonalIdText = null;
            LoanApplicationsForClient = null;
        }
    }
}
