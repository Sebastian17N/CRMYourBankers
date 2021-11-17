﻿namespace CRMYourBankers.Models
{
    public class BankClientPersonalLoan
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int BankId { get; set; }
        public Bank Bank { get; set; }
    }
}