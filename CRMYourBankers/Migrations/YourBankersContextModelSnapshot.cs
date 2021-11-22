﻿// <auto-generated />
using System;
using CRMYourBankers.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CRMYourBankers.Migrations
{
    [DbContext(typeof(YourBankersContext))]
    partial class YourBankersContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("CRMYourBankers.Models.Bank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Banks");
                });

            modelBuilder.Entity("CRMYourBankers.Models.BankClientPersonalLoan", b =>
                {
                    b.Property<int>("BankId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClientId")
                        .HasColumnType("INTEGER");

                    b.HasKey("BankId", "ClientId");

                    b.HasIndex("ClientId");

                    b.ToTable("BankClientPersonalLoans");
                });

            modelBuilder.Entity("CRMYourBankers.Models.Broker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Brokers");
                });

            modelBuilder.Entity("CRMYourBankers.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("AmountRequested")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BrokerId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClientCommission")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ClientStatus")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ContactPerson")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("GeneralNote")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<long?>("PersonalId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PhoneNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SourceOfIncome")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Spouse")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Us")
                        .HasColumnType("INTEGER");

                    b.Property<string>("WhatHesJob")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ZusUs")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BrokerId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("CRMYourBankers.Models.ClientTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClientId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Done")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("ClientTasks");
                });

            modelBuilder.Entity("CRMYourBankers.Models.LoanApplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AmountReceived")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AmountRequested")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BankId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ClientCommission")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClientId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LoanApplicationStatus")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LoanStartDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("MultiBrokerId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Paid")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BankId");

                    b.HasIndex("ClientId");

                    b.HasIndex("MultiBrokerId");

                    b.ToTable("LoanApplications");
                });

            modelBuilder.Entity("CRMYourBankers.Models.LoanTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Done")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LoanApplicationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LoanApplicationId");

                    b.ToTable("LoanTasks");
                });

            modelBuilder.Entity("CRMYourBankers.Models.MonthSummary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EstimatedTarget")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Month")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MonthSummaries");
                });

            modelBuilder.Entity("CRMYourBankers.Models.MultiBroker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MultiBrokers");
                });

            modelBuilder.Entity("CRMYourBankers.Models.BankClientPersonalLoan", b =>
                {
                    b.HasOne("CRMYourBankers.Models.Bank", "Bank")
                        .WithMany("PersonalLoanClients")
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CRMYourBankers.Models.Client", "Client")
                        .WithMany("ExistingPersonalLoans")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("CRMYourBankers.Models.Client", b =>
                {
                    b.HasOne("CRMYourBankers.Models.Broker", "Broker")
                        .WithMany("Clients")
                        .HasForeignKey("BrokerId");

                    b.Navigation("Broker");
                });

            modelBuilder.Entity("CRMYourBankers.Models.ClientTask", b =>
                {
                    b.HasOne("CRMYourBankers.Models.Client", "Client")
                        .WithMany("ClientTasks")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("CRMYourBankers.Models.LoanApplication", b =>
                {
                    b.HasOne("CRMYourBankers.Models.Bank", "Bank")
                        .WithMany("LoanApplications")
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CRMYourBankers.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CRMYourBankers.Models.MultiBroker", "MultiBroker")
                        .WithMany("LoanApplications")
                        .HasForeignKey("MultiBrokerId");

                    b.Navigation("Bank");

                    b.Navigation("Client");

                    b.Navigation("MultiBroker");
                });

            modelBuilder.Entity("CRMYourBankers.Models.LoanTask", b =>
                {
                    b.HasOne("CRMYourBankers.Models.LoanApplication", "LoanApplication")
                        .WithMany("LoanTasks")
                        .HasForeignKey("LoanApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LoanApplication");
                });

            modelBuilder.Entity("CRMYourBankers.Models.Bank", b =>
                {
                    b.Navigation("LoanApplications");

                    b.Navigation("PersonalLoanClients");
                });

            modelBuilder.Entity("CRMYourBankers.Models.Broker", b =>
                {
                    b.Navigation("Clients");
                });

            modelBuilder.Entity("CRMYourBankers.Models.Client", b =>
                {
                    b.Navigation("ClientTasks");

                    b.Navigation("ExistingPersonalLoans");
                });

            modelBuilder.Entity("CRMYourBankers.Models.LoanApplication", b =>
                {
                    b.Navigation("LoanTasks");
                });

            modelBuilder.Entity("CRMYourBankers.Models.MultiBroker", b =>
                {
                    b.Navigation("LoanApplications");
                });
#pragma warning restore 612, 618
        }
    }
}
