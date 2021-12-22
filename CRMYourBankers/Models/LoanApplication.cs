using CRMYourBankers.Enums;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CRMYourBankers.Models
{
    public class LoanApplication
    {        
        public int Id { get; set; }
        public int? AmountRequested { get; set; }
        public int? AmountReceived { get; set; }
        public int? ClientCommission { get; set; } 
        public int? BrokerCommission { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public Bank Bank { get; set; }
        public int BankId { get; set; }
        public int? MultiBrokerId { get; set; }
        public DateTime LoanStartDate { get; set; }
        public LoanApplicationStatus? LoanApplicationStatus { get; set; }
        public bool Paid { get; set; } = false; //można to też zapisać w konstruktorze, ustawić wartość domyślną
        public DateTime StartDate { get; set; }

        public MultiBroker MultiBroker { get; set; }
        public ObservableCollection<LoanTask> LoanTasks { get; set; }
        //public string TasksToDo => LoanTasks.LastOrDefault()?.Text;//wyciągnij ostatni z danej kolekcji
        //? zabezpiecza, że jeśli obiekt będzie pusty to nie odwołasz się do jego wnętrza

        public string TasksToDo 
            => string.Join(Environment.NewLine, 
                LoanTasks
                    .Where(task => !task.Done)
                    .Select(task => task.Text));       

        public LoanApplication()
        {
            LoanTasks = new ObservableCollection<LoanTask>();//dodając wniosek automatycznie tworzy sie pusta lista zadań
            StartDate = DateTime.Now;
        }

        public bool Validate()
        {
            return
                AmountRequested != null &&
                ClientId != 0 && // Jeśli ClientId lub BankId = 0 to oznacza, że nie wybrano ich z combo.
                BankId != 0 &&
                (ClientCommission == null || ClientCommission >= 0) &&
                (BrokerCommission == null || BrokerCommission >= 0) &&
                LoanApplicationStatus != null;
        }
    }
}
