using CRMYourBankers.Database;
using CRMYourBankers.Messages;
using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using CRMYourBankers.ViewModels.Interfaces;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CRMYourBankers.Enums;

namespace CRMYourBankers.ViewModels
{
    public class ResultViewModel : TabBaseViewModel, IRefreshReferenceDataOwner, IRefreshDataOwner
    {
        public ICommand DetailsScreenOpenHandler { get; set; }
        public ICommand SaveTargetButtonComand { get; set; }
        public ICommand AddNewMonthCommand { get; set; }
        public YourBankersContext Context { get; set; }
        public dynamic SelectedLoanApplication { get; set; }
        public int EstimatedTargetText { get; set; }
        
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
                NotifyPropertyChanged("RealizedScore");
                // jeśli kliknę w view w miesiąc to on wypełnia Value, potem SelectedMonthSummary
                // potem _selectedMonthSummary a NotifyPropertyChanged odświeża watości na view
                RefreshData();
            }
        }
    
        public string ActualScore =>
            SelectedMonthSummary != null ? ActualScoreValue.ToString() : "wybierz miesiąc";
        public int ActualScoreValue =>
            Context
                .LoanApplications
                .Where(loan => loan.LoanStartDate.Month == SelectedMonthSummary.Month.Month)
                .Sum(loan => loan.AmountReceived).Value;
        //to jest tylko getter, jeśli nie ma settera to na view musi być ustalone mode = one 
        //ponieważ dzięki temu kontrolki wiedzą, że to jest kmunikacja jednokierunkowa, view model na view
        public double RealizedScore => SelectedMonthSummary != null ?
            Math.Round(ActualScoreValue*100 / (double)SelectedMonthSummary.EstimatedTarget, 2) : 0;

        public ResultViewModel(Messenger messenger, YourBankersContext context) : 
            base(messenger, TabName.Result)
        {
            Context = context;
            RegisterCommands();
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
                                Id = loan.Id,
                                Paid = loan.Paid
                            }
                        ).ToList();

                NotifyPropertyChanged("DataGridData");
            }
        }
        public void RegisterCommands()
        {
            DetailsScreenOpenHandler = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = TabName.LoanApplicationDetails, 
                    ObjectId = SelectedLoanApplication.Id
                });
            });
            SaveTargetButtonComand = new RelayCommand(() =>
            {                
                if (!SelectedMonthSummary.Validate())
                {
                    MessageBox.Show("Niepoprawnie wypełnione dane lub puste pola",
                        "Błędy w formularzu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }
                else
                {
                    EstimatedTargetText = SelectedMonthSummary.EstimatedTarget;
                    MessageBox.Show($"Zapisano Target: {EstimatedTargetText}",
                    "Dodano Target",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);                    
                }
                Context.SaveChanges();
            });
            AddNewMonthCommand = new RelayCommand(() =>
            {
                if (Context.MonthSummaries.Any(month => 
                        month.Month.Year == DateTime.Today.Year &&
                        month.Month.Month == DateTime.Today.Month))
                {
                    MessageBox.Show("Obecny miesiąc już istnieje",
                       "Błędy w formularzu",
                       MessageBoxButton.OK,
                       MessageBoxImage.Warning);
                    return;
                }                
                var newMonthSummary = new MonthSummary
                {
                    Month = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)

                };
                Context.MonthSummaries.Add(newMonthSummary);
                Context.SaveChanges();
                MessageBox.Show($"Dodano nowy miesiąc: {newMonthSummary.Month: yyyy.MM}",
                    "Dodano Target",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            });
        }
    }
}
