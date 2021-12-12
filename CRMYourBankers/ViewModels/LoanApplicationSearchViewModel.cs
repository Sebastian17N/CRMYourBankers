using CRMYourBankers.Database;
using CRMYourBankers.Enums;
using CRMYourBankers.Messages;
using CRMYourBankers.ViewModels.Base;
using CRMYourBankers.ViewModels.Interfaces;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Input;

namespace CRMYourBankers.ViewModels
{
    public class LoanApplicationSearchViewModel : TabBaseViewModel, IRefreshDataOwner
    {     
        public ICommand SearchButtonCommand { get; set; }
        public ICommand DetailsScreenOpenHandler { get; set; }
        
        public dynamic DataGridData { get; set; }
        public dynamic SelectedLoanApplication { get; set; }

        public YourBankersContext Context { get; set; }
       
        public LoanApplicationSearchViewModel(Messenger messenger, YourBankersContext context) 
            : base(messenger, TabName.LoanApplicationSearch)
        {
            RegisterCommands();
            Context = context;                      
        }
        
        public void RefreshData()
        {
            DataGridData =           
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
                        loan.LoanApplicationStatus,
                        BankName = bank.Name,
                        loan.StartDate
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
                        loan.TasksToDo,
                        loan.LoanApplicationStatus,
                        StartDate = loan.StartDate,
                        StartDateString = loan.StartDate.ToString("MMMM yyyy").ToUpper()
                    })                
                .ToList();
            NotifyPropertyChanged("DataGridData");
        }

        public void RegisterCommands()
        {
            DetailsScreenOpenHandler = new RelayCommand(() =>
            {
                var selectedLoanApplicationId = (int)SelectedLoanApplication.Id;
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = TabName.LoanApplicationDetails,
                    SelectedObject = Context.LoanApplications.Single(loan => loan.Id == selectedLoanApplicationId),
                    LastTabName = TabName.LoanApplicationSearch
                });                
            });            
        }               
    }
}