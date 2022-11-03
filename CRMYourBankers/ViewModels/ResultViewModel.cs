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
    public class ResultViewModel : MonthlyFinancialStatementBase,
        IRefreshReferenceDataOwner, IRefreshDataOwner
    {
        public ICommand DetailsScreenOpenHandler { get; set; }
        public ICommand SaveTargetButtonComand { get; set; }
        public ICommand AddNewMonthCommand { get; set; }
        public ICommand AddNewYearCommand { get; set; }
        public dynamic SelectedLoanApplication { get; set; }
        public string EstimatedTargetText { get; set; }
        public dynamic DataGridData { get; set; }
        public dynamic DataGridDataYear { get; set; }

        public ObservableCollection<MonthSummary> MonthSummaries { get; set; }
        public ObservableCollection<YearSummary> YearSummaries { get; set; }

        public double CommissionPaid => SelectedMonthSummary != null ?
           Context.LoanApplications
           .Where(loan => loan.Paid)
           .Where(loan => loan.LoanStartDate.HasValue)
           .Where(loan => loan.LoanStartDate.Value.Month == SelectedMonthSummary.Month.Month)
           .Where(loan => loan.LoanStartDate.Value.Year == SelectedMonthSummary.Month.Year)
           .Sum(loan => loan.ClientCommission - loan.BrokerCommission).Value : 0;//value jest ponieważ clientCommision może być null i to zabezpiecza

        public double CommissionPaidYear => SelectedYearSummary != null ?
           Context.LoanApplications
           .Where(loan => loan.Paid)
           .Where(loan => loan.LoanStartDate.HasValue)
           .Where(loan => loan.LoanStartDate.Value.Year == SelectedYearSummary.Year.Year)
           .Sum(loan => loan.ClientCommission - loan.BrokerCommission).Value : 0;

        private MonthSummary _selectedMonthSummary;
        public MonthSummary SelectedMonthSummary 
        {
            get => _selectedMonthSummary;
            set
            {
                // jeśli kliknę w view w miesiąc to on wypełnia Value, potem SelectedMonthSummary
                // potem _selectedMonthSummary a NotifyPropertyChanged odświeża watości na view
                _selectedMonthSummary = value;
                if (SelectedMonthSummary != null)
                {
                    SelectedDateTime = SelectedMonthSummary.Month;
                }

                NotifyPropertyChanged("SelectedMonthSummary");                
                NotifyPropertyChanged("MonthSummaries");
                NotifyPropertyChanged("CommissionPaid");
                NotifyPropertyChanged("ActualScore");
                NotifyPropertyChanged("RealizedScore");
                RefreshData();
            }
        }
        
        private YearSummary _selectedYearSummary;
        public YearSummary SelectedYearSummary
        {
            get => _selectedYearSummary;
            set
            {
                _selectedYearSummary = value;
                if (_selectedYearSummary != null)
                {
                    SelectedDateTime = SelectedYearSummary.Year;
                }

                NotifyPropertyChanged("SelectedYearSummary");
                NotifyPropertyChanged("CommissionPaidYear");
                NotifyPropertyChanged("ActualYearScore");
                RefreshData();
            }
        }
        
        public ResultViewModel(Messenger messenger, YourBankersContext context) : 
            base(messenger, TabName.Result, context)
        {
            RegisterCommands();
        }     
        
        public void RefreshReferenceData()
        {
            MonthSummaries = new ObservableCollection<MonthSummary>(
                Context.MonthSummaries.ToList().OrderByDescending(month => month.Month.Date));

            YearSummaries = new ObservableCollection<YearSummary>(
                Context.YearSummaries.ToList().OrderByDescending(month => month.Year.Date));

            NotifyPropertyChanged("SelectedMonthSummary");
            NotifyPropertyChanged("SelectedYearSummary");
            NotifyPropertyChanged("MonthSummaries");
            NotifyPropertyChanged("YearSummaries");
        }
        //to odświeża się tylko gdy zmienia się TAB, jeśli chce to wywołać podczas pracy na konkretnej zakładki musze to dodać
        //tak jak w funkcji AddNewMonthCommand
        public void RefreshData()
        {
            if (SelectedMonthSummary != null)
            {
                DataGridData = 
                    Context
                        .LoanApplications
                        .Where(loan => loan.LoanApplicationStatus == LoanApplicationStatus.Launched)
                        .Where(loan => loan.LoanStartDate.HasValue)
                        .Where(loan =>
                            loan.LoanStartDate.Value.Month == SelectedMonthSummary.Month.Month)
                        .Where(loan =>
                            loan.LoanStartDate.Value.Year == SelectedMonthSummary.Month.Year)
                        .Select(loan =>
                            new
                            {
                                ClientFullName = loan.Client.FullName,
                                loan.AmountReceived,
                                BankName = loan.Bank.Name,
                                CommissionGet = loan.ClientCommission - loan.BrokerCommission,
                                MultiBroker = loan.MultiBroker.Name,
                                Id = loan.Id,
                                Paid = loan.Paid
                            }
                        ).ToList();
                
                NotifyPropertyChanged("DataGridData");

                EstimatedTargetText = SelectedMonthSummary.EstimatedTarget.ToString();
                NotifyPropertyChanged("EstimatedTargetText");
            }

            if (SelectedYearSummary != null)
            {
                DataGridDataYear =
                    Context
                        .LoanApplications
                        .Where(loan => loan.LoanApplicationStatus == LoanApplicationStatus.Launched)
                        .Where(loan => loan.LoanStartDate.HasValue)
                        .Where(loan => loan.LoanStartDate.Value.Year == SelectedYearSummary.Year.Year)
                        .Select(loan =>
                            new
                            {
                                loan.AmountReceived,
                                CommissionGet = loan.ClientCommission - loan.BrokerCommission,
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
                var selectedLoanApplicationId = (int)SelectedLoanApplication.Id;
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = TabName.LoanApplicationDetails,
                    SelectedObject = Context.LoanApplications.Single(loan => loan.Id == selectedLoanApplicationId)
                });
            });
            SaveTargetButtonComand = new RelayCommand(() =>
            {                
                if (!int.TryParse(EstimatedTargetText, out var estimatedTargetValue) 
                    || estimatedTargetValue < 0)
                {
                    MessageBox.Show("Niepoprawnie wypełnione dane lub puste pola",
                        "Błędy w formularzu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    EstimatedTargetText = SelectedMonthSummary.EstimatedTarget.ToString();
                    NotifyPropertyChanged("EstimatedTargetText");

                    return;
                }

                SelectedMonthSummary.EstimatedTarget = estimatedTargetValue;
                Context.SaveChanges();
                NotifyPropertyChanged("RealizedScore");

                MessageBox.Show($"Zapisano Target: {estimatedTargetValue}",
                    "Dodano Target",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);                   
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

                RefreshReferenceData();
            });

            AddNewYearCommand = new RelayCommand(() =>
            {
                if (Context.YearSummaries.Any(year =>
                        year.Year.Year == DateTime.Today.Year))
                {
                    MessageBox.Show("Obecny rok już istnieje",
                       "Błędy w formularzu",
                       MessageBoxButton.OK,
                       MessageBoxImage.Warning);
                    return;
                }
                var newYearSummary = new YearSummary
                {
                    Year = new DateTime(DateTime.Today.Year,1,1)
                };
                Context.YearSummaries.Add(newYearSummary);
                Context.SaveChanges();
                MessageBox.Show($"Dodano nowy rok: {newYearSummary.Year: yyyy}",
                    "Dodano Target",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                RefreshReferenceData();
            });
        }       
    }
}
