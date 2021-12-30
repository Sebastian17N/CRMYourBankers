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
using CRMYourBankers.Models.Interfaces;

namespace CRMYourBankers.ViewModels
{
	public class ClientDetailsViewModel : TabBaseViewModel, 
        IRefreshDataOwner, ILastTabNameOwner, IRefreshReferenceDataOwner, ISelectedItemOwner<Client>
    {
		private Client _selectedItem;
		public Client SelectedItem
		{
			get => _selectedItem;
			set
			{
				_selectedItem = value;

                if (_selectedItem != null)
                {
                    FirstNameText = _selectedItem.FirstName;
                    LastNameText = _selectedItem.LastName;
                    EmailText = _selectedItem.Email;
                    PhoneNumberText = _selectedItem.PhoneNumber;
                    PersonalIdText = _selectedItem.PersonalId;
                    AmountRequestedText = _selectedItem.AmountRequested;
                    ClientCommissionText = _selectedItem.ClientCommission;
                    ContactPersonText = _selectedItem.ContactPerson;
                    WhatHesJobText = _selectedItem.WhatHesJob;
                    SelectedZus = _selectedItem.ZusUs;
                    GeneralNoteText = _selectedItem.GeneralNote;
                    SelectedUs = _selectedItem.Us;
                    SelectedSpouse = _selectedItem.Spouse;
                    SelectedSourceOfIncome = _selectedItem.SourceOfIncome;
                    SelectedClientStatus = _selectedItem.ClientStatus;
                    BrokerId = _selectedItem.BrokerId ?? 0;
                    ClientTasks = _selectedItem.ClientTasks;
                    LoanApplicationsProposals = _selectedItem.LoanApplicationsProposalsInts;
                    BIKNoteText = _selectedItem.BIKNote;

                    ExistingPersonalLoans =
                     new ObservableCollection<BankClientBIK>(
                        _selectedItem
                        .ExistingBankClientBIK
                        .Where(loan => loan.BIKType == BIKType.PersonalLoans)
                        .ToList());
                    ExistingPersonalLoansQuestions =
                     new ObservableCollection<BankClientBIK>(
                        _selectedItem
                        .ExistingBankClientBIK
                        .Where(loan => loan.BIKType == BIKType.PersonalQuestions)
                        .ToList());
                    ExistingCompanyLoans =
                    new ObservableCollection<BankClientBIK>(
                        _selectedItem
                        .ExistingBankClientBIK
                        .Where(loan => loan.BIKType == BIKType.CompanyLoans)
                        .ToList());
                    ExistingCompanyLoansQuestions =
                    new ObservableCollection<BankClientBIK>(
                        _selectedItem
                        .ExistingBankClientBIK
                        .Where(loan => loan.BIKType == BIKType.CompanyQuestions)
                        .ToList());                    
                }
                else
                {
                    ClearAllFields();
                    ExistingPersonalLoans =
                    new ObservableCollection<BankClientBIK>();
                    ExistingPersonalLoansQuestions =
                    new ObservableCollection<BankClientBIK>();
                    ExistingCompanyLoans =
                    new ObservableCollection<BankClientBIK>();
                    ExistingCompanyLoansQuestions =
                    new ObservableCollection<BankClientBIK>();
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
        public DateTime? TaskDate { get; set; }
        public string BIKNoteText { get; set; }
        public List<int> LoanApplicationsProposals { get; set; }
        public TabName LastTabName { get; set; }
        public IEditable LastTabObject { get; set; }
        public ZusUs? SelectedZus { get; set; }
        public ZusUs? SelectedUs { get; set; }
        public Spouse? SelectedSpouse { get; set; }
        public SourceOfIncome? SelectedSourceOfIncome { get; set; }
        public ClientStatus? SelectedClientStatus { get; set; }
        public ObservableCollection<BankClientBIK> ExistingPersonalLoans { get; set; }
        public ObservableCollection<BankClientBIK> ExistingPersonalLoansQuestions { get; set; }
        public ObservableCollection<BankClientBIK> ExistingCompanyLoans { get; set; }
        public ObservableCollection<BankClientBIK> ExistingCompanyLoansQuestions { get; set; }
        public List<BankClientBIK> ExistingBankClientBIK => 
                                ExistingPersonalLoans
                                .Union(ExistingPersonalLoansQuestions)
                                .Union(ExistingCompanyLoans)
                                .Union(ExistingCompanyLoansQuestions)
                                .ToList();                                
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
                        .ThenBy(task => task.TaskDate)
						.ThenByDescending(task => task.Id)
						.ToList());
			}
		}

		private BankClientBIK _selectedElementBIKAnalysis;
		public BankClientBIK SelectedElementBIKAnalysis
		{
			get => _selectedElementBIKAnalysis;
			set
			{
				_selectedElementBIKAnalysis = value;
				NotifyPropertyChanged("SelectedElementBIKAnalysis");
				((RelayCommand<ObservableCollection<BankClientBIK>>)RemoveBIKAnalysisElementCommand).RaiseCanExecuteChanged();
			}
		}

		public ICommand SaveButtonCommand { get; set; }
		public ICommand CancelButtonCommand { get; set; }
		public ICommand DetailsScreenOpenHandler { get; set; }
		public ICommand AddNewClientTaskButtonCommand { get; set; }
		public ICommand AddNewExistingPersonalLoan { get; set; }
		public ICommand AddNewLoanApplicationCommand { get; set; }
		public ICommand RemoveBIKAnalysisElementCommand { get; set; }
        public ICommand ResetBIKButtonCommand { get; set; }


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
			if (SelectedItem == null)
			{
				return;
			}

            LoanApplicationsForClient =
                Context.LoanApplications
                    .Include(loan=>loan.LoanTasks)
                    .Where(loan => loan.ClientId == SelectedItem.Id) 
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
                ExistingPersonalLoans = ProcessCollectionBeforeSave(ExistingPersonalLoans, BIKType.PersonalLoans);
                ExistingPersonalLoansQuestions = ProcessCollectionBeforeSave(ExistingPersonalLoansQuestions, BIKType.PersonalQuestions);
                ExistingCompanyLoans = ProcessCollectionBeforeSave(ExistingCompanyLoans, BIKType.CompanyLoans);
                ExistingCompanyLoansQuestions = ProcessCollectionBeforeSave(ExistingCompanyLoansQuestions, BIKType.CompanyQuestions);

                var originalClientStatus = SelectedItem.ClientStatus;

                if (SelectedItem == null)
                {
                    SelectedItem = new Client
                    {
                        Id = Context.Clients.Max(client => client.Id) + 1,
                        SortIndex = Context.Clients.Max(client => client.SortIndex) + 1
                    };
                    Context.Clients.Add(SelectedItem);

                    //var newClient = new Client
                    //{
                    //    Id = Context.Clients.Max(client => client.Id) + 1,
                    //    SortIndex = Context.Clients.Max(client => client.SortIndex) + 1,
                    //    FirstName = FirstNameText,
                    //    LastName = LastNameText,
                    //    PhoneNumber = PhoneNumberText,
                    //    Email = EmailText,
                    //    PersonalId = PersonalIdText,
                    //    AmountRequested = AmountRequestedText,
                    //    ClientCommission = ClientCommissionText,
                    //    ContactPerson = ContactPersonText,
                    //    WhatHesJob = WhatHesJobText,
                    //    ZusUs = SelectedZus,
                    //    Us = SelectedUs,
                    //    GeneralNote = GeneralNoteText,
                    //    ExistingBankClientBIK = ExistingBankClientBIK,                       
                    //    Spouse = SelectedSpouse,
                    //    SourceOfIncome = SelectedSourceOfIncome,
                    //    ClientStatus = SelectedClientStatus,
                    //    BrokerId = BrokerId == 0 ? null : BrokerId,
                    //    LoanApplicationsProposalsInts = LoanApplicationsProposals,
                    //    BIKNote = BIKNoteText
                    //};
                    //Context.Clients.Add(newClient);                    
                }

                SelectedItem.FirstName = FirstNameText;
                SelectedItem.LastName = LastNameText;
                SelectedItem.PhoneNumber = PhoneNumberText;
                SelectedItem.Email = EmailText;
                SelectedItem.PersonalId = PersonalIdText;
                SelectedItem.AmountRequested = AmountRequestedText;
                SelectedItem.ClientCommission = ClientCommissionText;
                SelectedItem.ContactPerson = ContactPersonText;
                SelectedItem.WhatHesJob = WhatHesJobText;
                SelectedItem.ZusUs = SelectedZus;
                SelectedItem.Us = SelectedUs;
                SelectedItem.GeneralNote = GeneralNoteText;
                SelectedItem.ExistingBankClientBIK = ExistingBankClientBIK;
                SelectedItem.Spouse = SelectedSpouse;
                SelectedItem.SourceOfIncome = SelectedSourceOfIncome;
                SelectedItem.ClientStatus = SelectedClientStatus;
                SelectedItem.BrokerId = BrokerId == 0 ? null : BrokerId;
                SelectedItem.LoanApplicationsProposalsInts = LoanApplicationsProposals;
                SelectedItem.BIKNote = BIKNoteText;

                if (!SelectedItem.Validate())
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
                //            else
                //            {
                //                SelectedItem.FirstName = FirstNameText;
                //                SelectedItem.LastName = LastNameText;
                //                SelectedItem.PhoneNumber = PhoneNumberText;
                //                SelectedItem.Email = EmailText;
                //                SelectedItem.PersonalId = PersonalIdText;
                //                SelectedItem.AmountRequested = AmountRequestedText;
                //                SelectedItem.ClientCommission = ClientCommissionText;
                //                SelectedItem.ContactPerson = ContactPersonText;
                //                SelectedItem.WhatHesJob = WhatHesJobText;
                //                SelectedItem.ZusUs = SelectedZus;
                //                SelectedItem.GeneralNote = GeneralNoteText;
                //                SelectedItem.ExistingBankClientBIK = ExistingBankClientBIK;                            
                //                SelectedItem.Us = SelectedUs;
                //                SelectedItem.Spouse = SelectedSpouse;
                //                SelectedItem.SourceOfIncome = SelectedSourceOfIncome;
                //                SelectedItem.ClientStatus = SelectedClientStatus;
                //                SelectedItem.BrokerId = BrokerId == 0 ? null : BrokerId;
                //                SelectedItem.ClientTasks = ClientTasks;
                //                SelectedItem.BIKNote = BIKNoteText;
                //                SelectedItem.LoanApplicationsProposalsInts = LoanApplicationsProposals;

                if (originalClientStatus == ClientStatus.Active && SelectedItem.ClientStatus != ClientStatus.Active
                    && SelectedItem.ClientTasks.Any(task => task.TaskDate <= System.DateTime.Now && !task.Done))
                {
                    MessageBox.Show("Zmiana statusu Klienta niemożliwa, ponieważ istnieją zaległe zadania.",
                            "Błędy w formularzu",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                }                 
				Context.SaveChanges();
				MessageBox.Show($"Zapisano: {FirstNameText} {LastNameText}",
					"Dodano Klienta",
					MessageBoxButton.OK,
					MessageBoxImage.Information);

				TabMessenger.Send(new TabChangeMessage 
                { 
                    TabName = LastTabName,
                    SelectedObject = LastTabObject,
                    GoFurther = false
                });
				ClearAllFields();
			});

			CancelButtonCommand = new RelayCommand(() =>
			{
				ClearAllFields();
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = LastTabName,
                    SelectedObject = LastTabObject,
                    GoFurther = false
                });               

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
				if (SelectedItem != null)
				{
                    if (TaskDate <= DateTime.Now)
                    {
                        MessageBox.Show($"Nie można dodać zadania z datą wsteczną",
                            "Ostrzeżenie",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);

                        return;
                    }

                    using (var context = new YourBankersContext())
					{
                        var newClientTask = new ClientTask
						{
							Text = NewTaskText,
							ClientId = SelectedItem.Id,                            
                            TaskDate = TaskDate
						};
						context.ClientTasks.Add(newClientTask);
						context.SaveChanges();
					}

					SelectedItem.ClientTasks = new ObservableCollection<ClientTask>
						  (Context
							.ClientTasks
							.Where(task => task.ClientId == SelectedItem.Id)
							.ToList());
					ClientTasks = SelectedItem.ClientTasks;
				}

				NewTaskText = string.Empty;
                TaskDate = null;
				NotifyPropertyChanged("NewTaskText");
                NotifyPropertyChanged("TaskDate");
                NotifyPropertyChanged("ClientTasks");

                MessageBox.Show($"Nowe zadanie dodane",
					"Dodano Nowe Zadanie",
				   MessageBoxButton.OK,
				   MessageBoxImage.Information);
			});

			AddNewExistingPersonalLoan = new RelayCommand(() =>
			{
				if (ExistingPersonalLoans == null)
					ExistingPersonalLoans = new ObservableCollection<BankClientBIK>();

				ExistingPersonalLoans.Add(new BankClientBIK { ClientId = SelectedItem.Id });
				NotifyPropertyChanged("ExistingPersonalLoans");
			});

            RemoveBIKAnalysisElementCommand = new RelayCommand<ObservableCollection<BankClientBIK>>(
                collection =>
            {
                collection.Remove(SelectedElementBIKAnalysis);

            }, collection => SelectedElementBIKAnalysis != null);
            //przykład commanda z 2 parametrami

            AddNewLoanApplicationCommand = new RelayCommand<string>(loanProposalIndex => 
            {
                if (SelectedItem == null)
                    return;

				var loanProposalIndexValue = int.Parse(loanProposalIndex);

                if (LoanApplicationsProposals[loanProposalIndexValue] == 0)
                    return;

				var newLoanApplicationForClient = new LoanApplication
				{
					ClientId = SelectedItem.Id,
					Client = SelectedItem,
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

            ResetBIKButtonCommand = new RelayCommand(() =>
            {
                var result = MessageBox.Show("Czy na pewno resetować analizę BIK?",
                            "potwierdzenie czynności",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                    return;

                ExistingPersonalLoans.Clear();
                ExistingPersonalLoansQuestions.Clear();
                ExistingCompanyLoans.Clear();
                ExistingCompanyLoansQuestions.Clear();
                BIKNoteText = String.Empty;

                for(int i = 0; i < LoanApplicationsProposals.Count; i++)
                {
                    LoanApplicationsProposals[i] = 0;
                }

                NotifyPropertyChanged("ExistingPersonalLoans");
                NotifyPropertyChanged("ExistingPersonalLoansQuestions");
                NotifyPropertyChanged("ExistingCompanyLoans");
                NotifyPropertyChanged("ExistingCompanyLoansQuestions");
                NotifyPropertyChanged("LoanApplicationsProposals");
                NotifyPropertyChanged("BIKNoteText");

            });
        }

        private ObservableCollection<BankClientBIK> ProcessCollectionBeforeSave(
            ObservableCollection<BankClientBIK> collection, BIKType bikType)
        {
            var listWithoutRemovedItems = collection.ToList();
            listWithoutRemovedItems.RemoveAll(loan => loan.Bank == null);
            collection = new ObservableCollection<BankClientBIK>(listWithoutRemovedItems);

            foreach (var item in collection
                    .Where(loan => loan.BIKType != bikType))
            {
                item.BIKType = bikType;
            }

            return collection;
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
            ClientTasks = new ObservableCollection<ClientTask>();
            BIKNoteText = "";
            GeneralNoteText = "";
            BrokerId = 0;
            LoanApplicationsProposals = new List<int> { 0, 0, 0, 0, 0, 0, 0};
        }
    }
}
