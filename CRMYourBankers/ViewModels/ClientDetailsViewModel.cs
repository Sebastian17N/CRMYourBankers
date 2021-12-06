using CRMYourBankers.Database;
using CRMYourBankers.Messages;
using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using CRMYourBankers.ViewModels.Interfaces;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CRMYourBankers.Enums;
using System.Collections.ObjectModel;
using System;

namespace CRMYourBankers.ViewModels
{
    public class ClientDetailsViewModel : TabBaseViewModel, IRefreshDataOwner, ILastTabNameOwner, IRefreshReferenceDataOwner
    {
        private Client _selectedClients;
        public Client SelectedClient 
        { 
            get => _selectedClients; 
            set
            {
                _selectedClients = value;

                if (_selectedClients != null)
                {
                    FirstNameText = _selectedClients.FirstName;
                    LastNameText = _selectedClients.LastName;
                    EmailText = _selectedClients.Email;
                    PhoneNumberText = _selectedClients.PhoneNumber;
                    PersonalIdText = _selectedClients.PersonalId;
                    AmountRequestedText = _selectedClients.AmountRequested;
                    ClientCommissionText = _selectedClients.ClientCommission;
                    ContactPersonText = _selectedClients.ContactPerson;
                    WhatHesJobText = _selectedClients.WhatHesJob;
                    SelectedZus = _selectedClients.ZusUs;
                    GeneralNoteText = _selectedClients.GeneralNote;
                    ExistingPersonalLoans =
                     new ObservableCollection<BankClientBIK>(
                        _selectedClients
                        .ExistingBankClientBIK
                        .Where(loan => loan.BIKType == BIKType.PersonalLoans)
                        .ToList());
                    ExistingPersonalLoansQuestions =
                     new ObservableCollection<BankClientBIK>(
                        _selectedClients
                        .ExistingBankClientBIK
                        .Where(loan => loan.BIKType == BIKType.PersonalQuestions)
                        .ToList());
                    ExistingCompanyLoans =
                    new ObservableCollection<BankClientBIK>(
                        _selectedClients
                        .ExistingBankClientBIK
                        .Where(loan => loan.BIKType == BIKType.CompanyLoans)
                        .ToList());
                    ExistingCompanyLoansQuestions =
                    new ObservableCollection<BankClientBIK>(
                        _selectedClients
                        .ExistingBankClientBIK
                        .Where(loan => loan.BIKType == BIKType.CompanyQuestions)
                        .ToList());
                    SelectedUs = _selectedClients.Us;
                    SelectedSpouse = _selectedClients.Spouse;
                    SelectedSourceOfIncome = _selectedClients.SourceOfIncome;
                    SelectedClientStatus = _selectedClients.ClientStatus;
                    BrokerId = _selectedClients.BrokerId ??0;
                    ClientTasks = _selectedClients.ClientTasks;
                    LoanApplicationsProposals = _selectedClients.LoanApplicationsProposalsInts;
                    BIKNoteText = _selectedClients.BIKNote;
                }
                else
                {
                    ClearAllFields();
                }
            }
        }

        // Wydłużony zapis, tego co na dole dla FirstNameText
        //private string _firstNameText2;
        //public string FirstNameText2 
        //{
        //    get => _firstNameText2;
        //    set
        //    {
        //        _firstNameText2 = value;
        //    }
        //}

        public string FirstNameText { get; set; }
        public string LastNameText { get; set; }
        public string EmailText { get; set; }
        public int? PhoneNumberText { get; set; }
        public long? PersonalIdText { get; set; }
        public long? AmountRequestedText { get; set; }
        public string ClientCommissionText { get; set; }
        public string ContactPersonText { get; set; }
        public string WhatHesJobText { get; set; }
        public string GeneralNoteText { get; set; }
        public int BrokerId { get; set; }
        public string NewTaskText { get; set; }
        public string BIKNoteText { get; set; }
        public List<int> LoanApplicationsProposals { get; set; }
        public TabName LastTabName { get; set; }
        public ZusUs? SelectedZus { get; set; }
        public ZusUs? SelectedUs { get; set; }
        public Spouse? SelectedSpouse { get; set; }
        public SourceOfIncome? SelectedSourceOfIncome { get; set; }
        public ClientStatus? SelectedClientStatus { get; set; }

        public ObservableCollection<BankClientBIK> ExistingPersonalLoans { get; set; }
        public ObservableCollection<BankClientBIK> ExistingPersonalLoansQuestions { get; set; }
        public ObservableCollection<BankClientBIK> ExistingCompanyLoans { get; set; }
        public ObservableCollection<BankClientBIK> ExistingCompanyLoansQuestions { get; set; }

        public dynamic LoanApplicationsForClient { get; set; }

        private ObservableCollection<ClientTask> _clientTasks;
        public ObservableCollection<ClientTask> ClientTasks
        {
            get => _clientTasks;
            set 
            {
                _clientTasks = new ObservableCollection<ClientTask>(
                    value
                        .OrderBy(task => task.Done)
                        .ThenByDescending(task => task.Id)
                        .ToList());
            }
        }        

        public ICommand SaveButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand DetailsScreenOpenHandler { get; set; }
        public ICommand AddNewClientTaskButtonCommand { get; set; }
        public ICommand AddNewExistingPersonalLoan { get; set; }
        public ICommand AddNewLoanApplicationCommand { get; set; }
        public ICommand RemoveBIKAnalysisElementCommand { get; set; }

        public YourBankersContext Context { get; set; }
        public dynamic SelectedLoanApplication { get; set; }
        public ObservableCollection<Bank> Banks { get; set; }
        public ObservableCollection<Broker> Brokers { get; set; }

        public ClientDetailsViewModel(Messenger tabMessenger, YourBankersContext context)
            : base(tabMessenger, TabName.ClientDetails)
        {
            Context = context;
            RegisterCommands();
        }
        public void RefreshReferenceData()
        {
            Banks = new ObservableCollection<Bank>(Context.Banks.ToList());
            Brokers = new ObservableCollection<Broker>(Context.Brokers.ToList());
        }
        
        public void RefreshData()
        {
            if (SelectedClient == null)
            {
                return;
            }

            LoanApplicationsForClient =
                Context.LoanApplications
                    .Include(loan=>loan.LoanTasks)
                    .Where(loan => loan.ClientId == SelectedClient.Id) 
                    .Join(
                    Context.Banks,
                    loan => loan.BankId,
                    bank => bank.Id,
                    (loan, bank) => new
                    {
                        loan.Id,
                        loan.ClientId,
                        loan.AmountRequested,
                        loan.TasksToDo,
                        BankName = bank.Name
                    })
                .Join(
                    Context.Clients,
                    loan => loan.ClientId,
                    client => client.Id,
                    (loan, client) => new
                    {
                        loan.Id,
                        loan.BankName,
                        loan.AmountRequested,
                        loan.TasksToDo
                    }).ToList();
        }
        
        public void RegisterCommands()
        {
            SaveButtonCommand = new RelayCommand(() =>
            {
                foreach (var loan in ExistingPersonalLoans.Where(loan => loan.BankId == 0 || loan.Bank != null))
                    loan.BankId = loan.Bank.Id;

                foreach (var loan in ExistingPersonalLoans.Where(loan => loan.ClientId == 0))
                {
                    loan.ClientId = SelectedClient.Id;
                }

                var listWithoutRemovedItems = ExistingPersonalLoans.ToList();
                listWithoutRemovedItems.RemoveAll(loan => loan.BankId == 0);
                ExistingPersonalLoans = new ObservableCollection<BankClientBIK>(listWithoutRemovedItems);
                
                foreach (var item in ExistingPersonalLoansQuestions
                    .Where(loan => loan.BIKType != BIKType.PersonalQuestions))
                {
                    item.BIKType = BIKType.PersonalQuestions;
                }

                foreach (var item in ExistingCompanyLoans
                    .Where(loan => loan.BIKType != BIKType.CompanyLoans))
                {
                    item.BIKType = BIKType.CompanyLoans;
                }
                foreach (var item in ExistingCompanyLoansQuestions
                    .Where(loan => loan.BIKType != BIKType.CompanyQuestions))
                {
                    item.BIKType = BIKType.CompanyQuestions;
                }

                if (SelectedClient == null)
                {
                    
                    var newClient = new Client
                    {
                        Id = Context.Clients.Max(client => client.Id) + 1,
                        FirstName = FirstNameText,
                        LastName = LastNameText,
                        PhoneNumber = PhoneNumberText,
                        Email = EmailText,
                        PersonalId = PersonalIdText,
                        AmountRequested = AmountRequestedText,
                        ClientCommission = ClientCommissionText,
                        ContactPerson = ContactPersonText,
                        WhatHesJob = WhatHesJobText,
                        ZusUs = SelectedZus,
                        Us = SelectedUs,
                        GeneralNote = GeneralNoteText,
                        ExistingBankClientBIK = 
                                ExistingPersonalLoans
                                .Union(ExistingPersonalLoansQuestions)
                                .Union(ExistingCompanyLoans)
                                .Union(ExistingCompanyLoansQuestions)
                                .ToList(),                       
                        Spouse = SelectedSpouse,
                        SourceOfIncome = SelectedSourceOfIncome,
                        ClientStatus = SelectedClientStatus,
                        BrokerId = BrokerId == 0 ? null : BrokerId,
                        LoanApplicationsProposalsInts = LoanApplicationsProposals,
                        BIKNote = BIKNoteText
                    };

                    if (!newClient.Validate())
                    {
                        MessageBox.Show("Niepoprawnie wypełnione dane lub puste pola",
                            "Błędy w formularzu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;//nic nie zwraca tylko kończy funkcje/metode SaveButtonCommand (void)
                    }

                    if (Context.Clients.Any(item => item.PersonalId == PersonalIdText))
                    {
                        MessageBox.Show("Klient o podanym nr Pesel już istnieje",
                            "Błędy w formularzu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }

                    Context.Clients.Add(newClient);                    
                }
                else
                {
                    SelectedClient.FirstName = FirstNameText;
                    SelectedClient.LastName = LastNameText;
                    SelectedClient.PhoneNumber = PhoneNumberText;
                    SelectedClient.Email = EmailText;
                    SelectedClient.PersonalId = PersonalIdText;
                    SelectedClient.AmountRequested = AmountRequestedText;
                    SelectedClient.ClientCommission = ClientCommissionText;
                    SelectedClient.ContactPerson = ContactPersonText;
                    SelectedClient.WhatHesJob = WhatHesJobText;
                    SelectedClient.ZusUs = SelectedZus;
                    SelectedClient.GeneralNote = GeneralNoteText;
                    SelectedClient.ExistingBankClientBIK = 
                            ExistingPersonalLoans
                            .Union(ExistingPersonalLoansQuestions)
                            .Union(ExistingCompanyLoans)
                            .Union(ExistingCompanyLoansQuestions)
                            .ToList();
                    SelectedClient.Us = SelectedUs;
                    SelectedClient.Spouse = SelectedSpouse;
                    SelectedClient.SourceOfIncome = SelectedSourceOfIncome;
                    SelectedClient.ClientStatus = SelectedClientStatus;
                    SelectedClient.BrokerId = BrokerId == 0 ? null : BrokerId;
                    SelectedClient.ClientTasks = ClientTasks;
                    SelectedClient.BIKNote = BIKNoteText;
                    SelectedClient.LoanApplicationsProposalsInts = LoanApplicationsProposals;

                    if (!SelectedClient.Validate())
                    {
                        MessageBox.Show("Niepoprawnie wypełnione dane lub puste pola",
                            "Błędy w formularzu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;//nic nie zwraca tylko kończy funkcje/metode SaveButtonCommand (void)
                    }
                }

                Context.SaveChanges();
                MessageBox.Show($"Zapisano: {FirstNameText} {LastNameText}", 
                    "Dodano Klienta",   
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                TabMessenger.Send(new TabChangeMessage { TabName = LastTabName });
                ClearAllFields();                              
            });

            CancelButtonCommand = new RelayCommand(() =>
            {
                ClearAllFields();
                TabMessenger.Send(new TabChangeMessage { TabName = LastTabName });
            });

            DetailsScreenOpenHandler = new RelayCommand(() =>
            {
                var selectedLoanApplicationId = (int)SelectedLoanApplication.Id;
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = TabName.LoanApplicationDetails,
                    SelectedObject = Context.LoanApplications.Single(loan => loan.Id == selectedLoanApplicationId),
                    LastTabName = TabName.ClientDetails
                });
            });
            
            AddNewClientTaskButtonCommand = new RelayCommand(() =>
            {
                if (SelectedClient != null)
                {
                    using (var context = new YourBankersContext())
                    {
                        var newClientTask = new ClientTask
                        {
                            Text = NewTaskText,
                            ClientId = SelectedClient.Id
                        };

                        context.ClientTasks.Add(newClientTask);
                        context.SaveChanges();
                    }

                    SelectedClient.ClientTasks = new ObservableCollection<ClientTask>
                          (Context
                            .ClientTasks
                            .Where(task => task.ClientId == SelectedClient.Id)
                            .ToList());
                    ClientTasks = SelectedClient.ClientTasks;
                }

                NewTaskText = string.Empty;
                NotifyPropertyChanged("NewTaskText");

                MessageBox.Show($"Nowe zadanie dodane",
                    "Dodano Nowe Zadanie",
                   MessageBoxButton.OK,
                   MessageBoxImage.Information);
            });

            AddNewExistingPersonalLoan = new RelayCommand(() =>
            {
                if (ExistingPersonalLoans == null)
                    ExistingPersonalLoans = new ObservableCollection<BankClientBIK>();

                ExistingPersonalLoans.Add(new BankClientBIK { ClientId = SelectedClient.Id });
                NotifyPropertyChanged("ExistingPersonalLoans");
            });

            RemoveBIKAnalysisElementCommand = new RelayCommand(() =>
            {
                if (ExistingPersonalLoans == null)
                    return;

                //ExistingPersonalLoans.RemoveAt();

                NotifyPropertyChanged("ExistingPersonalLoans");
            });
            AddNewLoanApplicationCommand = new RelayCommand<string>(loanProposalIndex => 
            {
                if (SelectedClient == null)
                    return;

                var loanProposalIndexValue = int.Parse(loanProposalIndex);

                var newLoanApplicationForClient = new LoanApplication
                {                    
                    ClientId = SelectedClient.Id,
                    Client = SelectedClient,
                    BankId = LoanApplicationsProposals[loanProposalIndexValue],
                    Bank = Context.Banks.Single(bank => bank.Id == LoanApplicationsProposals[loanProposalIndexValue]),
                    LoanStartDate = DateTime.Now,
                    LoanApplicationStatus = LoanApplicationStatus.Submited
                };

                TabMessenger.Send(new TabChangeMessage
                {
                    SelectedObject = newLoanApplicationForClient,
                    TabName = TabName.LoanApplicationDetails,
                    LastTabName = TabName.ClientDetails
                });
            });               
        }

        public void ClearAllFields()
        {
            FirstNameText = "";
            LastNameText = "";
            PhoneNumberText = null;
            EmailText = "";
            ContactPersonText = "";
            WhatHesJobText = "";
            PersonalIdText = null;
            AmountRequestedText = null;
            ClientCommissionText = "";
            LoanApplicationsForClient = null;
            ExistingPersonalLoans = null;
            ExistingPersonalLoansQuestions = null;
            ExistingCompanyLoans = null;
            ExistingCompanyLoansQuestions = null;
            SelectedClientStatus = ClientStatus.InitiallyInterested;
            SelectedZus = null;
            SelectedUs = null;
            SelectedSpouse = null;
            SelectedSourceOfIncome = null;
            BIKNoteText = "";
            GeneralNoteText = "";
            BrokerId = 0;
            LoanApplicationsProposals = new List<int>();
        }
    }
}
