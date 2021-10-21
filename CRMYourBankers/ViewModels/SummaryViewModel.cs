using CRMYourBankers.Database;
using CRMYourBankers.Messages;
using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using CRMYourBankers.ViewModels.Interfaces;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CRMYourBankers.ViewModels
{
    public class SummaryViewModel : TabBaseViewModel, IRefreshDataOwner
    {        
        public ICommand LoanApplicationDetailsScreenOpenHandler { get; set; }
        public ICommand ClientDetailsScreenOpenHandler { get; set; }
        public dynamic SelectedLoanApplication { get; set; }

        public dynamic DataGridData { get; set; }
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

        public SummaryViewModel(Messenger messenger, YourBankersContext context) : base(messenger)
        {
            RegisterCommands();
            Context = context;
        }

        public void RefreshData()
        {
            DataGridData =
                Context.LoanApplications
                .Include(loan => loan.LoanTasks)
                .Join(Context.Banks,
                loan => loan.BankId,
                bank => bank.Id,
                (loan, bank) => new
                {
                    loan.Id,
                    loan.ClientId,
                    loan.AmountRequested,
                    loan.AmountReceived,
                    loan.TasksToDo,
                    BankName = bank.Name
                }).Join(
                    Context.Clients,
                    loan => loan.ClientId,
                    client => client.Id,
                    (loan, client) => new
                    {
                        loan.Id,
                        client.FullName,
                        loan.BankName,
                        loan.AmountRequested,
                        loan.AmountReceived,
                        loan.TasksToDo
                    })
                .ToList();
            
            
            Clients =
                Context
                    .Clients
                    .Include(client => client.ClientTasks)
                    .ToList();
            NotifyPropertyChanged("DataGridData");
        }


        public void RegisterCommands()
        {
            LoanApplicationDetailsScreenOpenHandler = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = "LoanApplicationDetails",
                    ObjectId = SelectedLoanApplication.Id
                });
            });

            ClientDetailsScreenOpenHandler = new RelayCommand(() =>
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
