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
                    LastName = LastNameText
                };

                if (newClient.Validate())
                {
                    Clients.Add(newClient);
                    MessageBox.Show($"Zapisano: {FirstNameText} {LastNameText}");
                }
                else
                {
                    MessageBox.Show("$Niepoprawnie wypełnione dane");
                }
            });

            CancelButtonCommand = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage { TabName = "ClientSearchTab" });
            });
        }
    }
}
