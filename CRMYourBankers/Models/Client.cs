using CRMYourBankers.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public long AmountRequested { get; set; }
        public string ClientCommission { get; set; }
        public string ContactPerson { get; set; }
        public string WhatHesJob { get; set; }
        public string GeneralNote { get; set; }
        public ZusUs? ZusUs { get; set; }
        public ZusUs? Us { get; set; }
        public Spouse? Spouse { get; set; }
        public SourceOfIncome? SourceOfIncome { get; set; }
        public ClientStatus? ClientStatus { get; set; }
        public int? BrokerId { get; set; }
        public ObservableCollection<ClientTask> ClientTasks { get; set; }
        public List<BankClientPersonalLoan> ExistingPersonalLoans { get; set; }
        //public List<BankClientPersonalLoan> ExistingPersonalLoansQuestions { get; set; }
        //public List<BankClientPersonalLoan> ExistingCompanyLoans { get; set; }
        //public List<BankClientPersonalLoan> ExistingCompanyLoansQuestions { get; set; }
        public Broker Broker { get; set; }

        public string FullName => $"{FirstName} {LastName} {Email}";
        public string TasksToDo => string.Join(Environment.NewLine,
            ClientTasks.Where(task => !task.Done).Select(task => task.Text));
        
        public Client()
        {
            ClientTasks = new ObservableCollection<ClientTask>();
            ExistingPersonalLoans = new List<BankClientPersonalLoan>();
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
