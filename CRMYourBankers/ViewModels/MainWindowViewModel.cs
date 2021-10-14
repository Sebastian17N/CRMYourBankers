using CRMYourBankers.Messages;
using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CRMYourBankers.ViewModels
{
    public class MainWindowViewModel : TabBaseViewModel
    {
        // 1. Dodaj 3 nowe pola dla Client: nr telefonu, pesel, mail.
        // 2. Użytkownik może dodawać te pola z interfaceu.
        // 3. Użytkownik może zobaczyć nowe pola jako kolumny na ClientSearch.
        // 4. Za każdym wejściem w 'AddNewClient' pola powinny być puste.
        // 5. Przycisk Save na ClientDetails powinien wyświetlić MessageBox (zrobione) i przejśc do ClientSearch.
        // 6. Nie można kliknąć Save jeśli pola są puste ALBO istnieje już użytkownik o tym samym nr PESEL.
        // 7. Pozostałe walidacje na użytkowniku - rozwiązanie systemowe. Metoda "bool Validate()" w Client.
        // powtórzyć walidację Validate, co dokładnie robi?
        // powtórzyć Invoke, co dokładnie robi?
        // co oznacza PropertyChangedEventHandler
        // dokładne zastosowanie klas base i interfaces
        // 8. Dodać logikę edycji klienta. Czyli jeśli ekran jest otwarty z już istniejącym
        //         klientem, to walidacja peselu musi nie brać pod uwagę, wpisu który edytujesz (LINQ).
        //TODO: 1. Dodać nowy ekran do tworzenia/ edytowania wniosków, do którego można przejść
        //         z ekranu edycji klienta. Ma działać Bank lista, po dodaniu dodaniu nowej LoanApplication, powinno mieć przypisane
        //            BankId i ClientId,
        //         w widoku klineta poniżej ma wyświetlać się lista aktuanych wniosków    
        //         dodać validację wniosku
        //         dodaj przycisk, który wywoła linię 23 w LoanApplications = PrepareData(loanApplications, banks, clients);
        //         czyli ponownie przeładuje zbiór danych (odświeży)
        //         możesz otworzyć istniejący wniosek i go edytować/zapisać => jak przekazać w SelectedLoanApplication do bombobox liste banków i klientów???

        //   Dlaczego w klientach jest puste pole a w wnioskach nie?
        //   Zadania w widoku wniosków powinny być w formie tabelki i powinny być edytowalne tak jak w szczegóły klienta, nie ustawiać kolumn jako readonly

        public ICommand OpenClientsSearchScreenCommand { get; set; }
        public ICommand OpenLoanApplicationsSearchScreenCommand { get; set; }
        public ICommand AddNewClientButtonCommand { get; set; }
        public ICommand AddNewLoanApplicationCommand { get; set; }

        public ObservableCollection<object> ItemTabs => _itemTabs;
        private ObservableCollection<object> _itemTabs = new ObservableCollection<object>();
        //można wymiennie pisać z NotifyPropertyChanged

        private LoanApplicationSearchViewModel _loanApplicationSearchViewModel;
        private ClientSearchViewModel _clientSearchViewModel;
        private ClientDetailsViewModel _clientDetailsViewModel;
        private LoanApplicationDetailsViewModel _loanApplicationDetailsViewModel;
        
        public List<Client> Clients { get; set; }
        public List<LoanApplication> LoanApplications { get; set; }
        public List<Bank> Banks { get; set; }
        public List<LoanTask> LoanTasks { get; set; }

        private object _selectedTab;
        public object SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                NotifyPropertyChanged("SelectedTab");
            }
        }

        public MainWindowViewModel()
        {
            TabMessenger = new Messenger();

            Clients = new List<Client>
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
            };
            LoanApplications = new List<LoanApplication>
            {
                new LoanApplication
                {
                    Id = 1,
                    ClientId = 1,
                    BankId = 3,
                    AmountRequested = 100000,
                    AmountReceived = 100000,
                    ClientCommission = 5000,
                    LoanTasks = new List<LoanTask>
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
                    }
                },
                new LoanApplication
                {
                    Id = 2,
                    ClientId = 2,
                    BankId = 4,
                    AmountRequested = 200000,
                    AmountReceived = 200000,
                    ClientCommission = 10000,                    
                }
            };
            Banks = new List<Bank>
            {
                new Bank{Id = 1, Name = "Santander"},
                new Bank{Id = 2, Name = "Alior"},
                new Bank{Id = 3, Name = "BNP"},
                new Bank{Id = 4, Name = "mBank"},
            };
            RegisterCommands();
            RegisterMessages();

            _loanApplicationSearchViewModel = new LoanApplicationSearchViewModel(TabMessenger,
                LoanApplications, Banks, Clients);
            _itemTabs.Add(_loanApplicationSearchViewModel);

            _clientSearchViewModel = new ClientSearchViewModel(Clients, TabMessenger);
            _itemTabs.Add(_clientSearchViewModel);

            _clientDetailsViewModel = new ClientDetailsViewModel(TabMessenger, Clients, LoanApplications, Banks);
            _itemTabs.Add(_clientDetailsViewModel);

            _loanApplicationDetailsViewModel = new LoanApplicationDetailsViewModel(TabMessenger, LoanApplications, Clients, Banks, LoanTasks);
            _itemTabs.Add(_loanApplicationDetailsViewModel);

            SelectedTab = _loanApplicationSearchViewModel;
        }

        protected void RegisterCommands()
        {
            AddNewClientButtonCommand = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage { TabName = "ClientDetails" });
            });
            AddNewLoanApplicationCommand = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage { TabName = "LoanApplicationDetails" });
            });
            OpenClientsSearchScreenCommand = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage { TabName = "ClientSearch" });
            });
            OpenLoanApplicationsSearchScreenCommand = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage { TabName = "LoanApplicationSearch" });
            });
        }

        public void RegisterMessages()
        {
            TabMessenger.Register<TabChangeMessage>(this,
                message =>
            {
                _clientSearchViewModel.TabVisibility = Visibility.Collapsed;
                _clientDetailsViewModel.TabVisibility = Visibility.Collapsed;

                switch (message.TabName)
                {
                    case "ClientDetails":
                        _clientDetailsViewModel.TabVisibility = Visibility.Visible;
                        SelectedTab = _clientDetailsViewModel;
                        _clientDetailsViewModel.SelectedClient = message.Client;
                        break;

                    case "ClientSearch":
                        _clientSearchViewModel.TabVisibility = Visibility.Visible;
                        SelectedTab = _clientSearchViewModel;
                        break;

                    case "LoanApplicationDetails":
                        _loanApplicationDetailsViewModel.TabVisibility = Visibility.Visible;
                        SelectedTab = _loanApplicationDetailsViewModel;

                        if (message.ObjectId > 0)
                        {
                            _loanApplicationDetailsViewModel.SelectedLoanApplication =
                                LoanApplications.Single(loan => loan.Id == message.ObjectId);
                        }
                        
                        _loanApplicationDetailsViewModel.SelectedLoanTasks = message.LoanTask;
                                               
                        break;

                    case "LoanApplicationSearch":
                        _loanApplicationSearchViewModel.TabVisibility = Visibility.Visible;
                        SelectedTab = _loanApplicationSearchViewModel;
                        break;
                }
            });
        }
    }
}
