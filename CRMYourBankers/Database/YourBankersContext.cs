﻿using CRMYourBankers.Models;
using Microsoft.EntityFrameworkCore;
using System;
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
        public DbSet<Broker> Brokers { get; set; }
        public DbSet<MultiBroker> MultiBrokers { get; set; }
        public DbSet<LoanTask> LoanTasks { get; set; }
        public DbSet<ClientTask> ClientTasks { get; set; }
        public DbSet<MonthSummary> MonthSummaries { get; set; }
        public DbSet<YearSummary> YearSummaries { get; set; }
        public DbSet<BankClientBIK> BankClientBIK { get; set; }
        public DbSet<LoanApplicationsProposal> LoanApplicationsProposals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "DataSource=" + (File.Exists("CRMYourBankers.csproj") ? "YourBankersConnection.db" : "./../../../YourBankersConnection.db");
            //var connectionString = "DataSource=Database/YourBankersConnection.db";

            optionsBuilder.UseSqlite(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BankClientBIK>()
                .HasKey(bc => new { bc.BankId, bc.ClientId, bc.BIKType });
            //zbiór tych 3 wartości musi być unikalny, tak jakby kazdy w Polsce miał 3 pesele
            //1 element może być dzielony miedzy rekory, 2 i 3 też, ale żaden rekord nie ma jednocześnie 
            //wszystkich 3 takich samych elementów jak inny rekord
            //może być tylko jeden: mBank, Jan Kowalski, Kredyt Osobisty
            modelBuilder.Entity<LoanApplicationsProposal>()
                .HasKey(prop => new { prop.ClientId, prop.ProposalIndex });
        }

        public void DataSeeds()
        {
            AddBanks();
            AddBrokers();
            AddMultiBrokers();            
        }

        public void TestSeeds()
		{
			AddClients();
			AddClientTasks();
			AddLoanApplications();
			AddLoanTasks();
			AddMonthSummaries();
            AddYearummaries();
            AddLoanApplicationsProposals();
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
                            PhoneNumber = "888777999",
                            Email = "zielinski@wp.pl",
                            PersonalId = 12121212345
                        },
                        new Client
                        {
                            Id = 2,
                            FirstName = "Jan",
                            LastName ="Kowalski",
                            PhoneNumber = "555444666",
                            Email = "kowalski@onet.pl",
                            PersonalId = 55443312345
                        }
                    });
                SaveChanges();
            }
        }

        private void AddClientTasks()
        {
            if (!ClientTasks.Any())
            {
                ClientTasks.AddRange(
                    new List<ClientTask>
                    {
                        new ClientTask
                        {
                            Id = 1,
                            Text = "Zadzwoń w piątek",
                            Done = false,
                            ClientId = 1
                        },
                        new ClientTask
                        {
                            Id = 2,
                            Text = "Wyślij maila z ofertą",
                            Done = false,
                            ClientId = 1
                        }
                    });
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
                        StartDate = DateTime.Parse("2021/11/12")
                    },
                    new LoanApplication
                    {
                        Id = 2,
                        ClientId = 2,
                        BankId = 4,
                        AmountRequested = 200000,
                        AmountReceived = 200000,
                        ClientCommission = 10000,
                        StartDate = DateTime.Parse("2021/10/22")
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

        private void AddBrokers()
        {
            if (!Brokers.Any())
            {
                Brokers.AddRange(
                    new List<Broker>
                    {
                        new Broker{Id = 1, Name = "Maciej Kwiatkowski"},
                        new Broker{Id = 2, Name = "Jakub Nieroda"},
                        new Broker{Id = 3, Name = "Ola Nieroda"},
                        new Broker{Id = 4, Name = "Anna Borowik"},
                        new Broker{Id = 5, Name = "Radosław Dominiak"},
                    });
                SaveChanges();
            }
        }

        private void AddMultiBrokers()
        {
            if (!MultiBrokers.Any())
            {
                MultiBrokers.AddRange(
                    new List<MultiBroker>
                    {
                        new MultiBroker{Id = 1, Name = "Lendi"},
                        new MultiBroker{Id = 2, Name = "Open"},
                        new MultiBroker{Id = 3, Name = "Brak"},
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
                        new Bank{Id = 1, Name = "SANTANDER"},
                        new Bank{Id = 2, Name = "SANTANDER Firmowy"},
                        new Bank{Id = 3, Name = "ALIOR"},
                        new Bank{Id = 4, Name = "ALIOR Firmowy"},
                        new Bank{Id = 5, Name = "mBANK"},
                        new Bank{Id = 6, Name = "mBANK Firmowy"},
                        new Bank{Id = 7, Name = "BNP"},
                        new Bank{Id = 8, Name = "BNP Firmowy"},
                        new Bank{Id = 9, Name = "Pocztowy"},
                        new Bank{Id = 10, Name = "GET IN"},
                        new Bank{Id = 11, Name = "AION"},
                        new Bank{Id = 12, Name = "CITI"},
                        new Bank{Id = 13, Name = "CA"},
                        new Bank{Id = 14, Name = "BOŚ"},
                        new Bank{Id = 15, Name = "SKOK"},
                        new Bank{Id = 16, Name = "ING"},
                        new Bank{Id = 17, Name = "NEST"},
                        new Bank{Id = 18, Name = "SANTANDER CONSUMER"},
                        new Bank{Id = 19, Name = "PEKAO"},
                        new Bank{Id = 20, Name = "PKO BP"},
                        new Bank{Id = 21, Name = "HIPOTEKA"},
                        new Bank{Id = 22, Name = "UPADŁOŚĆ"}
                    });
                SaveChanges();
            }
        }

        private void AddMonthSummaries()
		{
			if (MonthSummaries.Any())
				return;

			MonthSummaries.AddRange(
				new MonthSummary { Month = DateTime.Parse("2021/09/01"), EstimatedTarget = 1024000 },
				new MonthSummary { Month = DateTime.Parse("2021/10/01"), EstimatedTarget = 2024000 },
				new MonthSummary { Month = DateTime.Parse("2021/11/01"), EstimatedTarget = 3024000 }
				);
			SaveChanges();
		}
        private void AddYearummaries()
        {
            if (YearSummaries.Any())
                return;

            YearSummaries.AddRange(
                new YearSummary { Year = DateTime.Parse("2021") }
                );
            SaveChanges();
        }

        private void AddLoanApplicationsProposals()
		{
            if (LoanApplicationsProposals.Any())
                return;

            foreach (var client in Clients)
            {
                for (var proposalIndex = 0; proposalIndex < 7; ++proposalIndex)
                {
                    client.LoanApplicationsProposals.Add(
                        new LoanApplicationsProposal
                        {
                            ClientId = client.Id,
                            BankId = null,
                            ProposalIndex = proposalIndex
                        });
                }
            }
            SaveChanges();
		}
    }
}