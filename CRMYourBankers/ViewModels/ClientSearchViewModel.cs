using CRMYourBankers.Database;
using CRMYourBankers.Messages;
using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public YourBankersContext Context { get; set; }

        public Client SelectedClient { get; set; }
        
        public ClientSearchViewModel(Messenger messenger, YourBankersContext context)
            : base(messenger)
        {
            Context = context;
            RegisterCommands();
        }

        protected override void RefreshData()
        {
            Clients =
                Context
                    .Clients
                    .Include(client => client.ClientTasks)
                    .ToList();
        }

        public void RegisterCommands()
        {
            DetailsScreenOpenHandler = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = "ClientDetails",
                    ObjectId = SelectedClient.Id
                });
            });
        }
    }
}
