using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Windows.Input;

namespace CRMYourBankers.ViewModels
{
    public class ClientSearchViewModel : TabBaseViewModel
    {
        public ICommand SearchButtonCommand { get; set; }

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
        public ClientSearchViewModel(List<Client> clients)
        {
            Clients = clients;
            RegisterCommands();
        }

        public void RegisterCommands()
        {
            SearchButtonCommand = new RelayCommand(() =>
            {
                NotifyPropertyChanged("Clients");
            });
        }
    }
}
