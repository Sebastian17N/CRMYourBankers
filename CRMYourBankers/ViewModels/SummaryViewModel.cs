using CRMYourBankers.Database;
using CRMYourBankers.Messages;
using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
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
    public class SummaryViewModel : TabBaseViewModel
    {        
        public ICommand DetailsScreenOpenHandler { get; set; }
        public dynamic SelectedLoanApplication { get; set; }

        public dynamic DataGridData { get; set; }

        public YourBankersContext Context { get; set; }
              
        public SummaryViewModel(Messenger messenger, YourBankersContext context) : base(messenger)
        {
            RegisterCommands();
            Context = context;
        }

        protected override void RefreshData()
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
            NotifyPropertyChanged("DataGridData");              
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

    }
}
