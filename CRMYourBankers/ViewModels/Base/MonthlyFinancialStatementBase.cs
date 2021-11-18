using CRMYourBankers.Database;
using CRMYourBankers.Enums;
using CRMYourBankers.Models;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMYourBankers.ViewModels.Base
{
    public class MonthlyFinancialStatementBase: TabBaseViewModel
    {
        public YourBankersContext Context { get; set; }        
        public DateTime SelectedDateTime { get; set; }

        public string ActualScore =>
           SelectedDateTime != DateTime.MinValue ? ActualScoreValue.ToString() : "wybierz miesiąc";
        public int ActualScoreValue =>
           Context
               .LoanApplications
               .Where(loan =>
                       loan.LoanStartDate.Year == SelectedDateTime.Year &&
                       loan.LoanStartDate.Month == SelectedDateTime.Month)
               .Sum(loan => loan.AmountReceived).Value;
        public int ActualTarget =>
            Context
                .MonthSummaries
                .Where(target =>
                        target.Month.Month == SelectedDateTime.Month &&
                        target.Month.Year == SelectedDateTime.Year)
                .Select(target => target.EstimatedTarget)
                .SingleOrDefault();//wyciągnij pojedynczą wartość albo domyślną jeśli nie znajdziesz wartości
        public double RealizedScore => ActualScoreValue != 0 ?
            Math.Round(ActualScoreValue * 100 / (double)ActualTarget, 2) : 0;
        public MonthlyFinancialStatementBase(Messenger messenger, TabName tabName, YourBankersContext context)
           : base(messenger, tabName)
        {
            Context = context;
        }
    }
}
