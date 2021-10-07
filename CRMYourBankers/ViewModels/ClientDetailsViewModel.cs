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

        private Client _client;
        public Client SelectedClient 
        { 
            get => _client; 
            set
            {
                _client = value;
                if (_client != null)
                {
                    FirstNameText = _client.FirstName;
                    LastNameText = _client.LastName;
                    EmailText = _client.Email;
                    PhoneNumberText = _client.PhoneNumber;
                    PersonalIdText = _client.PersonalId;
                }                
            }
        }

        public string FirstNameText { get; set; }
        public string LastNameText { get; set; }
        public string EmailText { get; set; }
        public int? PhoneNumberText { get; set; }
        public long? PersonalIdText { get; set; }

        public ICommand SaveButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }

        public ClientDetailsViewModel(Messenger tabMessenger, List<Client> clients)
            : base(tabMessenger)
        {
            Clients = clients;
            RegisterCommands();
        }

        public void RegisterCommands()
        {
            SaveButtonCommand = new RelayCommand(() =>
            {
                var newClient = new Client
                {
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
                MessageBox.Show($"Zapisano: {FirstNameText} {LastNameText}", 
                    "Dodano Klienta",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                TabMessenger.Send(new TabChangeMessage { TabName = "ClientSearch" });
                ClearAllFields();
                              
            });

            CancelButtonCommand = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage { TabName = "ClientSearch" });
            });

        }
        public void EditClient()
        {
                    
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
