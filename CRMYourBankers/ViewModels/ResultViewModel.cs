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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CRMYourBankers.ViewModels
{
    public class ResultViewModel : TabBaseViewModel, IRefreshReferenceDataOwner, IRefreshDataOwner
    {
        public ICommand DetailsScreenOpenHandler { get; set; }
        public YourBankersContext Context { get; set; }

        public List<string> YesNoList = new()
        {
            "Tak",
            "Nie"
        };

        public dynamic DataGridData { get; set; }

        public ObservableCollection<MonthSummary> MonthSummaries { get; set; }

        private MonthSummary _selectedMonthSummary;
        public MonthSummary SelectedMonthSummary 
        {
            get => _selectedMonthSummary;
            set
            {
                _selectedMonthSummary = value;
                NotifyPropertyChanged("SelectedMonthSummary");
                NotifyPropertyChanged("ActualScore");
               
                RefreshData();
            }
        }

        public string ActualScore =>
            SelectedMonthSummary != null ?
            Context
                .LoanApplications
                .Where(loan => loan.LoanStartDate.Month == SelectedMonthSummary.Month.Month)
                .Sum(loan => loan.AmountReceived).ToString() :
            "wybierz miesiąc";

        public ResultViewModel(Messenger messenger, YourBankersContext context) : base(messenger)
        {
            Context = context;
            RegisterCommands();
        }
        
        public void RegisterCommands()
        {
            DetailsScreenOpenHandler = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = "Result"
                });
            });
        }

        public void RefreshReferenceData()
        {
            MonthSummaries = new ObservableCollection<MonthSummary>(Context.MonthSummaries.ToList());
            
        }

        public void RefreshData()
        {
            if (SelectedMonthSummary != null)
            {
                DataGridData =
                    Context
                        .LoanApplications
                        .Where(loan =>
                            loan.LoanStartDate.Month == SelectedMonthSummary.Month.Month)
                        .Select(loan =>
                            new
                            {
                                ClientFullName = loan.Client.FullName,
                                loan.AmountReceived,
                                BankName = loan.Bank.Name,
                                ClientCommission = loan.ClientCommission,
                                YesNoList
                            }
                        ).ToList();

                NotifyPropertyChanged("DataGridData");
                NotifyPropertyChanged("YesNoList");
            }
        }
    }
}
