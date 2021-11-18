using CRMYourBankers.Database;
using CRMYourBankers.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMYourBankers.ViewModels.Base
{
    public class MonthlyFinancialStatementBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public YourBankersContext Context { get; set; }
        public MonthSummary SelectedMonthSummary { get; set; }
        public string ActualScore =>
           SelectedMonthSummary != null ? ActualScoreValue.ToString() : "wybierz miesiąc";
        public int ActualScoreValue =>
            Context
                .LoanApplications
                .Where(loan => loan.LoanStartDate.Month == SelectedMonthSummary.Month.Month)
                .Where(loan => loan.LoanStartDate.Year == SelectedMonthSummary.Month.Year)
                .Sum(loan => loan.AmountReceived).Value;
        //to jest tylko getter, jeśli nie ma settera to na view musi być ustalone mode = one 
        //ponieważ dzięki temu kontrolki wiedzą, że to jest kmunikacja jednokierunkowa, view model na view
        public double RealizedScore => SelectedMonthSummary != null ?
            Math.Round(ActualScoreValue * 100 / (double)SelectedMonthSummary.EstimatedTarget, 2) : 0;
        public double CommissionPaid => SelectedMonthSummary != null ?
            Context.LoanApplications
            .Where(loan => loan.Paid)
            .Where(loan => loan.LoanStartDate.Month == SelectedMonthSummary.Month.Month)
            .Where(loan => loan.LoanStartDate.Year == SelectedMonthSummary.Month.Year)
            .Sum(loan => loan.ClientCommission).Value : 0;//value jest ponieważ clientCommision może być null i to zabezpiecza
    }
}
