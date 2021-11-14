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
    public class SummaryViewModel : TabBaseViewModel, 
        IRefreshReferenceDataOwner, IRefreshDataOwner, IMonthlyFinancialStatementOwner
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
                NotifyPropertyChanged("SelectedMonthSummary");
                RefreshData();
                MonthlyFinancialStatement();
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
                .SingleOrDefault();//wyciągnij pojedynczą wartość albo domyślną jeśli nie znajdziesz wartości
        public double RealizedScore => ActualScoreValue != 0 ? 
            Math.Round(ActualScoreValue * 100 / (double)ActualTarget, 2) : 0;

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
                        loan.TasksToDo,
                        //client.BankId
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
                    TabName = TabName.LoanApplicationDetails,
                    ObjectId = SelectedLoanApplication.Id,
                    LastTabName = TabName.Summary
                });
            });

            ClientDetailsScreenOpenHandler = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = TabName.ClientDetails,
                    ObjectId = SelectedClient.Id,
                    LastTabName = TabName.Summary                    
                });
            });            
        }

        public void RefreshReferenceData()
        {
            MonthSummaries = new ObservableCollection<MonthSummary>(Context.MonthSummaries.ToList());
        }

        public void MonthlyFinancialStatement()
        {            
            NotifyPropertyChanged("ActualScoreValue");
        }
    }
}
