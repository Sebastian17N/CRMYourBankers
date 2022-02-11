using CRMYourBankers.Database;
using CRMYourBankers.Enums;
using CRMYourBankers.Messages;
using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using CRMYourBankers.ViewModels.Interfaces;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CRMYourBankers.ViewModels
{
    public class SummaryViewModel : MonthlyFinancialStatementBase, IRefreshReferenceDataOwner, IRefreshDataOwner
    {
        public ICommand LoanApplicationDetailsScreenOpenHandler { get; set; }
        public ICommand ClientDetailsScreenOpenHandler { get; set; }

        public ICommand MoveClientUpCommand { get; set; }
        public ICommand MoveClientDownCommand { get; set; }

        public dynamic SelectedLoanApplication { get; set; }
        public ObservableCollection<MonthSummary> MonthSummaries { get; set; }

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
        public Client SelectedClient { get; set; }
        public dynamic DataGridScore { get; set; }

        public string ArrowUpPath => $"{AppDomain.CurrentDomain.BaseDirectory}Images\\ArrowUp.png";
        public string ArrowDownPath => $"{AppDomain.CurrentDomain.BaseDirectory}Images\\ArrowDown.png";


        // Przykłady innego napisania RealizedScore w postaci property z widocznym get i funkcji.
        //public double Costam => ActualScoreValue != 0 ?
        //    ActualScoreValue * 100 / ActualTarget : 0;

        //public double RealizedScoreProperty
        //{
        //    get
        //    {
        //        if (ActualScoreValue != 0)
        //            return Math.Round(ActualScoreValue * 100 / (double)ActualTarget, 2);
        //        else
        //            return 0;
        //    }
        //}

        //public double RealizedScoreFunction()
        //{
        //    if (ActualScoreValue != 0)
        //        return Math.Round(ActualScoreValue * 100 / (double)ActualTarget, 2);
        //    else
        //        return 0;
        //}

        //strzałka to lambda, można powiedzieć, że to funkcja a nawet properta
        //funkcja anonimowa to funkcja, która nie ma nazwy i nie możesz się do niej odwołać
        public SummaryViewModel(Messenger messenger, YourBankersContext context) :
            base(messenger, TabName.Summary, context)
        {
            RegisterCommands();
            Context = context;
        }

        public void RefreshData()
        {
            SelectedDateTime = DateTime.Today;

            DataGridData =
                Context.LoanApplications
                .Where(loan => loan.LoanApplicationStatus == LoanApplicationStatus.Offer ||
                               loan.LoanApplicationStatus == LoanApplicationStatus.Submited ||
                               loan.LoanApplicationStatus == LoanApplicationStatus.CreditDecision)
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
                        loan.TasksToDo,
                    })                
                .ToList()
                .OrderBy(client => client.FullName);
                
            NotifyPropertyChanged("DataGridData");

            RefreshClients();

            DataGridScore =
                Context
                    .LoanApplications
                    .Where(loan =>
                        loan.LoanStartDate.HasValue &&
                        loan.LoanStartDate.Value.Year == DateTime.Today.Year &&
                        loan.LoanStartDate.Value.Month == DateTime.Today.Month)
                    .Select(loan =>
                        new
                        {
                            ClientFullName = loan.Client.FullName,
                            loan.AmountReceived,
                            BankName = loan.Bank.Name,
                            ClientCommission = loan.ClientCommission,
                            Id = loan.Id
                        }
                    ).ToList();
            NotifyPropertyChanged("DataGridScore");
        }

        public void RefreshClients()
		{
            Clients =
                Context
                    .Clients
                    .Where(client => client.ClientStatus == ClientStatus.Active ||
                                     client.ClientStatus == ClientStatus.InitiallyInterested)
                    .Include(client => client.ClientTasks)
                    .Include(client => client.ExistingBankClientBIK)
                    .Include(client => client.LoanApplicationsProposals)
                    .OrderByDescending(client => client.SortIndex)
                    .ToList();

            NotifyPropertyChanged("Clients");
        }

        public void RegisterCommands()
        {
            LoanApplicationDetailsScreenOpenHandler = new RelayCommand(() =>
            {
                var selectedLoanApplicationId = (int)SelectedLoanApplication.Id;
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = TabName.LoanApplicationDetails,
                    SelectedObject = Context.LoanApplications.Single(loan => loan.Id == selectedLoanApplicationId)
                });
            });

            ClientDetailsScreenOpenHandler = new RelayCommand(() =>
            {
                var selectedClientId = (int)SelectedClient.Id;
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = TabName.ClientDetails,
                    SelectedObject = Context.Clients.Single(client => client.Id == selectedClientId)
                });
            });

            MoveClientUpCommand = new RelayCommand(() =>
            {

                var aaa = Context
                        .Clients
                        .Where(client =>
                            (client.ClientStatus == ClientStatus.Active || client.ClientStatus == ClientStatus.InitiallyInterested)
                            && client.SortIndex < SelectedClient.SortIndex);

                var clientToSwitch =
                    Context
                        .Clients
                        .Where(client =>
                            (client.ClientStatus == ClientStatus.Active || client.ClientStatus == ClientStatus.InitiallyInterested)
                            && client.SortIndex < SelectedClient.SortIndex)
                        .OrderByDescending(client => client.SortIndex)
                        .FirstOrDefault();

                if (clientToSwitch == null)
                    return;

                var oldSortIndex = SelectedClient.SortIndex;
                SelectedClient.SortIndex = clientToSwitch.SortIndex;
                clientToSwitch.SortIndex = oldSortIndex;

                Context.SaveChanges();
                RefreshClients();
            });

            MoveClientDownCommand = new RelayCommand(() =>
            {
                var clientToSwitch =
                    Context
                        .Clients
                        .Where(client =>
                            (client.ClientStatus == ClientStatus.Active || client.ClientStatus == ClientStatus.InitiallyInterested)
                            && client.SortIndex > SelectedClient.SortIndex)
                        .OrderBy(client => client.SortIndex)
                        .FirstOrDefault();

                if (clientToSwitch == null)
                    return;

                var oldSortIndex = SelectedClient.SortIndex;
                SelectedClient.SortIndex = clientToSwitch.SortIndex;
                clientToSwitch.SortIndex = oldSortIndex;

                Context.SaveChanges();
                RefreshClients();
            });
        }

        public void RefreshReferenceData()
        {
            MonthSummaries = new ObservableCollection<MonthSummary>(Context.MonthSummaries.ToList());
        }
    }
}
