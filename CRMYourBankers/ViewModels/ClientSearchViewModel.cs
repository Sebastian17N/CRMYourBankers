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
using System.Windows.Input;
using CRMYourBankers.Enums;

namespace CRMYourBankers.ViewModels
{
    public class ClientSearchViewModel : TabBaseViewModel, IRefreshDataOwner
    {
        public ICommand SearchButtonCommand { get; set; }
        public ICommand DetailsScreenOpenHandler { get; set; }
        public string SearchText { get; set; }

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
            : base(messenger, TabName.ClientSearch)
        {
            Context = context;
            RegisterCommands();
        }

        public void RefreshData()
        {
            var query =
                Context
                    .Clients
                    .Include(client => client.ClientTasks)
                    .Include(bank => bank.ExistingBankClientBIK)
                    .Include(proposal => proposal.LoanApplicationsProposals)
                    .AsQueryable();

            if (!string.IsNullOrEmpty(SearchText))
                query = query
                    .Where(client => EF.Functions.Like(client.FirstName + " " + client.LastName + " " + client.Email, $"%{SearchText}%"));

            Clients = query.ToList();
        }

        public void RegisterCommands()
        {
            DetailsScreenOpenHandler = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = TabName.ClientDetails,
                    SelectedObject = SelectedClient,
                    LastTabName = TabName.ClientSearch
                });
            });

			SearchButtonCommand = new RelayCommand(() =>
			{
                RefreshData();
			});
		}
    }
}
