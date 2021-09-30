using CRMYourBankers.Messages;
using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace CRMYourBankers.ViewModels
{
    public class ClientDetailsViewModel : TabBaseViewModel
    {
        public List<Client> Clients { get; set; }

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

            if (
                newClient.Validate() &&
                FirstNameText != "" &&
                LastNameText != "" &&
                PhoneNumberText != null &&
                EmailText != "" &&
                PersonalIdText != null
                )
                {                    
                    Clients.Add(newClient);
                    MessageBox.Show($"Zapisano: {FirstNameText} {LastNameText}");
                    TabMessenger.Send(new TabChangeMessage { TabName = "ClientSearchTab" });
                    FirstNameText = "";
                    LastNameText = "";
                    PhoneNumberText = null;
                    EmailText = "";
                    PersonalIdText = null;
                }
                else
                {
                    MessageBox.Show("Niepoprawnie wypełnione dane lub puste pola");
                }
            });

            CancelButtonCommand = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage { TabName = "ClientSearchTab" });
            });
        }
    }
}
