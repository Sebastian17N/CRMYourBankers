using CRMYourBankers.Messages;
using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CRMYourBankers.ViewModels
{
    public class ClientDetailsViewModel : TabBaseViewModel
    {
        public List<Client> Clients { get; set; }
        public List<LoanApplication> LoanApplications { get; set; }
        public List<Bank> Banks { get; set; }

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

                    PrepareLoanApplicationDataForClient();
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
            set { _clientTasks = value;
                NotifyPropertyChanged("ClientTask");
            }
        }        
        public ICommand SaveButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }

        public ClientDetailsViewModel(Messenger tabMessenger, List<Client> clients,
            List<LoanApplication> loanApplications, List<Bank> banks, List<ClientTask> clientTasks)
            : base(tabMessenger)
        {
            Clients = clients;
            LoanApplications = loanApplications;
            Banks = banks;
            ClientTasks = clientTasks;

            RegisterCommands();
        }

        private void PrepareLoanApplicationDataForClient()
        {
            LoanApplicationsForClient =
                LoanApplications
                    .Where(loan => loan.ClientId == SelectedClient.Id)
                    .Join(
                    Banks,
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
                    Clients,
                    loan => loan.ClientId,
                    client => client.Id,
                    (loan, client) => new
                    {
                        loan.BankName,
                        loan.AmountRequested,
                        loan.TasksToDo
                    });
        }    

        public void RegisterCommands()
        {
            SaveButtonCommand = new RelayCommand(() =>
            {
                if (SelectedClient == null)
                {
                    var newClient = new Client
                    {
                        Id = Clients.Max(client => client.Id) + 1,
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

                    if (Clients.Any(item => item.PersonalId == PersonalIdText))
                    {
                        MessageBox.Show("Klient o podanym nr Pesel już istnieje",
                            "Błędy w formularzu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }

                    Clients.Add(newClient);
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
        }
    }
}
