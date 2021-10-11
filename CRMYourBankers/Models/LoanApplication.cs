using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMYourBankers.Models
{
    public class LoanApplication
    {        
        public int Id { get; set; }
        public int? AmountRequested { get; set; }
        public int? AmountReceived { get; set; }
        public int? ClientCommission { get; set; }             
        public int ClientId { get; set; }
        public int BankId { get; set; }
        public List<LoanTask> LoanTasks { get; set; }
        //public string TasksToDo => LoanTasks.LastOrDefault()?.Text;//wyciągnij ostatni z danej kolekcji
        //? zabezpiecza, że jeśli obiekt będzie pusty to nie odwołasz się do jego wnętrza
        public string TasksToDo => string.Join(Environment.NewLine, LoanTasks.Select(loan => loan.Text));
        public LoanApplication()
        {
            LoanTasks = new List<LoanTask>();//dodając wniosek automatycznie tworzy sie pusta lista zadań
        }

        public bool Validate()
        {
            return
                AmountRequested != null&&
                ClientId != 0 &&
                BankId != 0;
        }
    }
}
