﻿using CRMYourBankers.Models;
using CRMYourBankers.Models.Maps;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CRMYourBankers.Database
{
    public class DbContext : System.Data.Entity.DbContext
    {
        public DbContext() : base("name=YourBankersConnection") { }

        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ClientMap());
            base.OnModelCreating(modelBuilder);
        }

        public void DataSeeds()
        {
            if (!Clients.Any())
            {
                Clients.AddRange(
                    new List<Client>
                    {
                        new Client
                        {
                            Id = 1,
                            FirstName = "Piotr",
                            LastName ="Zieliński",
                            PhoneNumber = 888777999,
                            Email = "zielinski@wp.pl",
                            PersonalId = 12121212345
                        },
                        new Client
                        {
                            Id = 2,
                            FirstName = "Jan",
                            LastName ="Kowalski",
                            PhoneNumber = 555444666,
                            Email = "kowalski@onet.pl",
                            PersonalId = 55443312345
                        }
                    }
                );
            }
        }
    }
}
