using CRMYourBankers.Messages;
using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
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

        public List<Client> Clients { get; set; }
        public List<LoanApplication> LoanApplications { get; set; }
        public List<Bank> Banks { get; set; }

        public LoanApplicationSearchViewModel(Messenger messenger,
            List<LoanApplication> loanApplications, List<Bank> banks, List<Client> clients) 
            : base(messenger)
        {
            RegisterCommands();

            LoanApplications = loanApplications;
            Clients = clients;
            Banks = banks;
        }

        private dynamic PrepareData(List<LoanApplication> loanApplications, List<Bank> banks, List<Client> clients)
        {
            return
                loanApplications.Join(
                    banks,
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
                    clients,
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
                    });
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
            DataGridData = PrepareData(LoanApplications, Banks, Clients);
            NotifyPropertyChanged("LoanApplications");
        }
    }
}