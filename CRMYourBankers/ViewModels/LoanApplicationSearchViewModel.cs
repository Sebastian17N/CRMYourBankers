﻿using CRMYourBankers.Database;
using CRMYourBankers.Messages;
using CRMYourBankers.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Input;

namespace CRMYourBankers.ViewModels
{
    public class LoanApplicationSearchViewModel : TabBaseViewModel
    {     
        public ICommand SearchButtonCommand { get; set; }
        public ICommand DetailsScreenOpenHandler { get; set; }
        
        public dynamic DataGridData { get; set; }
        public dynamic SelectedLoanApplication { get; set; }

        public YourBankersContext Context { get; set; }
       
        public LoanApplicationSearchViewModel(Messenger messenger, YourBankersContext context) 
            : base(messenger)
        {
            RegisterCommands();
            Context = context;

            DataGridData = PrepareData();
            NotifyPropertyChanged("DataGridData");
        }

        private dynamic PrepareData()
        {
            return
                Context
                .LoanApplications
                .Include(loan => loan.LoanTasks)
                .Join(
                    Context.Banks,
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
                    })
                .Join(
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
        }

        public void RegisterCommands()
        {
            DetailsScreenOpenHandler = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = "LoanApplicationDetails",
                    ObjectId = SelectedLoanApplication.Id
                });                
            });            
        }

        protected override void RefreshData()
        {
            DataGridData = PrepareData();
            NotifyPropertyChanged("DataGridData");
        }
    }
}