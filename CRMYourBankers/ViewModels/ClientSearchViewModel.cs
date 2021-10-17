using CRMYourBankers.Messages;
using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Windows.Input;

namespace CRMYourBankers.ViewModels
{
    public class ClientSearchViewModel : TabBaseViewModel
    {
        public ICommand SearchButtonCommand { get; set; }
        public ICommand DetailsScreenOpenHandler { get; set; }

        private List<Client> _clients;
        public List<Client> Clients 
        { 
            get => _clients; 
            set
            {
                _clients = value;
                NotifyPropertyChanged("Clients");
            }
        }

        public Client SelectedClient { get; set; }
        
        public ClientSearchViewModel(List<Client> clients, Messenger messenger)
            : base(messenger)
        {
            Clients = clients;
            RegisterCommands();
        }

        public void RegisterCommands()
        {
            //SearchButtonCommand = new RelayCommand(() =>
            //{
            //    NotifyPropertyChanged("Clients");
            //});

            DetailsScreenOpenHandler = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = "ClientDetails",
                    Client = SelectedClient
                });
            });
        }
    }
}
