using System;
using System.Collections.Generic;
using System.Linq;

namespace CRMYourBankers.Models
{
    public class Client
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? PhoneNumber { get; set; }
        public string Email { get; set; }        
        public long? PersonalId { get; set; }
        public int Id { get; set; }
        public List<ClientTask> ClientTasks { get; set; }

        public string FullName => $"{FirstName} {LastName} {Email}";
        public string TasksToDo => string.Join(Environment.NewLine,
            ClientTasks.Where(task => !task.Done).Select(task => task.Text));
        
        public Client()
        {
            ClientTasks = new List<ClientTask>();
        }
        public bool Validate()
            //validate sprawdza poprawność 
        {
            return
            FirstName != "" &&
            LastName != "" &&
            PhoneNumber != null &&
            Email != "" &&
            PersonalId != null;
        }       
    }
}
