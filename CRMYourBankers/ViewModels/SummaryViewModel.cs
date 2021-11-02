using CRMYourBankers.Database;
using CRMYourBankers.Enum;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CRMYourBankers.ViewModels
{
    public class SummaryViewModel : TabBaseViewModel, IRefreshReferenceDataOwner, IRefreshDataOwner
    {        
        public ICommand LoanApplicationDetailsScreenOpenHandler { get; set; }
        public ICommand ClientDetailsScreenOpenHandler { get; set; }
        public dynamic SelectedLoanApplication { get; set; }
        public ObservableCollection<MonthSummary> MonthSummaries { get; set; }

        public dynamic DataGridData { get; set; }
        public dynamic DataGridScore { get; set; }
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
       
        private MonthSummary _selectedMonthSummary;
        public MonthSummary SelectedMonthSummary
        {
            get => _selectedMonthSummary; 
            set
            {
                _selectedMonthSummary = value;
                NotifyPropertyChanged("ActualScoreValue");
                NotifyPropertyChanged("SelectedMonthSummary");
                RefreshData();
            }
        }
        
        public int ActualScoreValue =>
            Context
                .LoanApplications
                .Where(loan => 
                        loan.LoanStartDate.Year == DateTime.Today.Year &&
                        loan.LoanStartDate.Month == DateTime.Today.Month)
                .Sum(loan => loan.AmountReceived).Value;
        public int ActualTarget =>
            Context
                .MonthSummaries
                .Where(target => 
                        target.Month.Month == DateTime.Today.Month &&
                        target.Month.Year == DateTime.Today.Year)
                .Select(target => target.EstimatedTarget)
                .SingleOrDefault();//wyciągnij pojedynczą wartość albo domyślną jeśli nei znajdziesz wartości

        public SummaryViewModel(Messenger messenger, YourBankersContext context) : 
            base(messenger, TabName.Summary)
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
                      
            DataGridScore =
                Context
                    .LoanApplications
                    .Where(loan =>
                        loan.LoanStartDate.Year == DateTime.Today.Year &&
                        loan.LoanStartDate.Month == DateTime.Today.Month)
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

        public void RegisterCommands()
        {
            LoanApplicationDetailsScreenOpenHandler = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = "LoanApplicationDetails",
                    ObjectId = SelectedLoanApplication.Id,
                    LastTabName = TabName.Summary
                });
            });

            ClientDetailsScreenOpenHandler = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = "ClientDetails",
                    ObjectId = SelectedClient.Id,
                    LastTabName = TabName.Summary                    
                });
            });            
        }

        public void RefreshReferenceData()
        {
            MonthSummaries = new ObservableCollection<MonthSummary>(Context.MonthSummaries.ToList());
        }
    }
}
