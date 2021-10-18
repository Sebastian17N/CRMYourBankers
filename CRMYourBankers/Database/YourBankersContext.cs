using CRMYourBankers.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CRMYourBankers.Database
{
    public class YourBankersContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<LoanApplication> LoanApplications { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<LoanTask> LoanTasks { get; set; }
        public DbSet<ClientTask> ClientTasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "DataSource=" +
                (File.Exists("YourBankersConnection.db") ? "YourBankersConnection.db" : "./../../../YourBankersConnection.db");

            optionsBuilder.UseSqlite(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public void DataSeeds()
        {
            AddClients();
            AddLoanApplications();
            AddLoanTasks();
            AddBanks();
        }

        private void AddClients()
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

                SaveChanges();
            }
        }

        private void AddLoanApplications()
        {
            if (!LoanApplications.Any())
            {
                LoanApplications.AddRange(
                    new LoanApplication
                    {
                        Id = 1,
                        ClientId = 1,
                        BankId = 3,
                        AmountRequested = 100000,
                        AmountReceived = 100000,
                        ClientCommission = 5000,
                    },
                    new LoanApplication
                    {
                        Id = 2,
                        ClientId = 2,
                        BankId = 4,
                        AmountRequested = 200000,
                        AmountReceived = 200000,
                        ClientCommission = 10000,
                    });

                SaveChanges();
            }
        }

        private void AddLoanTasks()
        {
            if (!LoanTasks.Any())
            {
                LoanTasks.AddRange(
                    new List<LoanTask>
                        {
                            new LoanTask
                            {
                                Id = 1,
                                Text = "Zadzwoń do klienta",
                                Done = false,
                                LoanApplicationId = 1
                            },
                            new LoanTask
                            {
                                Id = 2,
                                Text = "Wyślij wniosek do Banku",
                                Done = false,
                                LoanApplicationId = 1
                            }
                        });

                SaveChanges();
            }
        }

        private void AddBanks()
        {
            if (!Banks.Any())
            {
                Banks.AddRange(
                    new List<Bank>
                    {
                        new Bank{Id = 1, Name = "Santander"},
                        new Bank{Id = 2, Name = "Alior"},
                        new Bank{Id = 3, Name = "BNP"},
                        new Bank{Id = 4, Name = "mBank"},
                    });

                SaveChanges();
            }
        }
    }
}
