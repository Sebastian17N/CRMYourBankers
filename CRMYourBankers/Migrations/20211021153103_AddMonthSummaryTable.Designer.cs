﻿// <auto-generated />
using System;
using CRMYourBankers.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CRMYourBankers.Migrations
{
    [DbContext(typeof(YourBankersContext))]
    [Migration("20211021153103_AddMonthSummaryTable")]
    partial class AddMonthSummaryTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("CRMYourBankers.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<long?>("PersonalId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PhoneNumber")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

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

                    b.HasKey("Id");

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

            modelBuilder.Entity("CRMYourBankers.Models.ClientTask", b =>
                {
                    b.HasOne("CRMYourBankers.Models.Client", "Client")
                        .WithMany("ClientTasks")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
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

            modelBuilder.Entity("CRMYourBankers.Models.Client", b =>
                {
                    b.Navigation("ClientTasks");
                });

            modelBuilder.Entity("CRMYourBankers.Models.LoanApplication", b =>
                {
                    b.Navigation("LoanTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
