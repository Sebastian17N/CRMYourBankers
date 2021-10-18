using CRMYourBankers.Database;
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

        //  Co chcesz mieć na Summary i w jakiej formie
        //  Stworzyć ClientTask i dodać jego wyświetlanie na ClientDetails
        //  Kontrolki => dodawanie dowych tasków??? może być poprzez TextBox oraz kliknięcie myszką w listę zadań, pojawi się pytanie czy chce dodać nowe zadanie
        // Problemy: dlaczego w Client Details na początki wyświetla się tylko jedna lista zadań, a po odświeżeniu 2


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
        private SummaryViewModel _summaryViewModel;
        
        public List<Client> Clients { get; set; }
        public List<LoanApplication> LoanApplications { get; set; }
        public List<Bank> Banks { get; set; }
        public List<LoanTask> LoanTasks { get; set; }
        public List<ClientTask> ClientTasks { get; set; }

        public YourBankersContext Context { get; set; }

        private TabBaseViewModel _selectedTab;
        public TabBaseViewModel SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                _selectedTab.TabVisibility = Visibility.Visible;
                NotifyPropertyChanged("SelectedTab");

                // Rzutowanie
                //var text = "Jakiś tekst";
                //var otherText = 29.8;
                // Potraktuj to co jest w otherText jak int (więc utnij część po przecinku i zostaw 29)
                //var number = (int)otherText;

                // Traktuj _selectedTab jak obiekt typu TabBaseViewModel
                //((TabBaseViewModel)_selectedTab)
            }
        }

        public MainWindowViewModel()
        {
            TabMessenger = new Messenger();

            Context = new YourBankersContext();
            Context.DataSeeds();

            // TODO: Pozbyć się tych przypisań.
            Clients = Context.Clients.ToList();
            LoanApplications = Context.LoanApplications.ToList();
            Banks = Context.Banks.ToList();            

            RegisterCommands();
            RegisterMessages();

            _loanApplicationSearchViewModel = new LoanApplicationSearchViewModel(TabMessenger, Context);
            _itemTabs.Add(_loanApplicationSearchViewModel);

            _clientSearchViewModel = new ClientSearchViewModel(TabMessenger, Context);
            _itemTabs.Add(_clientSearchViewModel);

            _clientDetailsViewModel = new ClientDetailsViewModel(TabMessenger, Clients, LoanApplications, Banks, ClientTasks);
            _itemTabs.Add(_clientDetailsViewModel);

            _loanApplicationDetailsViewModel = new LoanApplicationDetailsViewModel(TabMessenger, LoanApplications, Clients, Banks, LoanTasks);
            _itemTabs.Add(_loanApplicationDetailsViewModel);
            
            _summaryViewModel = new SummaryViewModel(TabMessenger, LoanApplications, Banks, Clients);
            _itemTabs.Add(_summaryViewModel);

            SelectedTab = _summaryViewModel; //to okno wyświetla się jako pierwsze
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
                switch (message.TabName)
                {
                    case "ClientSearch":
                        SelectedTab = _clientSearchViewModel;
                        break;

                    case "ClientDetails":
                        SelectedTab = _clientDetailsViewModel;
                        _clientDetailsViewModel.SelectedClient = 
                            Context.Clients.Single(client => client.Id == message.ObjectId);
                        break;

                    case "LoanApplicationSearch":
                        SelectedTab = _loanApplicationSearchViewModel;
                        break;

                    case "LoanApplicationDetails":
                        SelectedTab = _loanApplicationDetailsViewModel;

                        if (message.ObjectId > 0)
                        {
                            _loanApplicationDetailsViewModel.SelectedLoanApplication =
                                LoanApplications.Single(loan => loan.Id == message.ObjectId);
                        }                                                                                             
                        break;

                    case "SummaryViewModel":
                        SelectedTab = _summaryViewModel;
                        break;
                }
            });
        }
    }
}
