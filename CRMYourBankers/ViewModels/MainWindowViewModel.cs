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
        // Uzupełnij wszystkie widoki w context
        // Problemy:Dlaczego we wniosku nie wyświetla się lista banków i klientów?
        // nowy klient/wniosek ma być pusty i logika otwierania aktualnego klienta
        // Result View mają się wyświetlać wszystkie pola
        // Summary View ma działać przechodzenie do innych widoków z gridów

        //Główne menu zaprogramować kontorlki te same co w Result (żeby wybrać aktualny miesiąc wybierz DateTime.Today(można użyć do dodaj nowy wniosek i zamiast null to data dzisiejsza(zmien dateTime.minvalue)))
        //jaką informację potrzebujesz żeby po kliknięcu cancel wracało do vidoku w którym przed chwilą byłeś, pomyś nie musisz robić
        //można by zrobić jakąś zmienną pocniczą Bool i jeżeli przekchodzę z tab name "jakieś" to wypełnia się true albo false, potem wciskając cancel odwołuje się do tej zmiennej
        //Stwórz przycisk na wynikach, który pozwoli na dodawanie nowego mc
        //zasatanów się gdzie będzie edytowany cel na dany mc
        //Dla zus i us stwórz enum, oddzielny folder => class, 
        //Problemy: jak wyciągnąć target dla obecnego miesiąca? bez użycia selectedMonth
        //          dlaczego textbox phonenumber wyrzuca błąd?
        public ICommand OpenClientsSearchScreenCommand { get; set; }
        public ICommand OpenLoanApplicationsSearchScreenCommand { get; set; }
        public ICommand AddNewClientButtonCommand { get; set; }
        public ICommand AddNewLoanApplicationCommand { get; set; }
        public ICommand OpenMainWindowSearchScreenCommand { get; set; }
        public ICommand OpenResultScreenCommand { get; set; }

        public ObservableCollection<object> ItemTabs => _itemTabs;
        private ObservableCollection<object> _itemTabs = new ObservableCollection<object>();
        //można wymiennie pisać z NotifyPropertyChanged

        private LoanApplicationSearchViewModel _loanApplicationSearchViewModel;
        private ClientSearchViewModel _clientSearchViewModel;
        private ClientDetailsViewModel _clientDetailsViewModel;
        private LoanApplicationDetailsViewModel _loanApplicationDetailsViewModel;
        private SummaryViewModel _summaryViewModel;
        private ResultViewModel _resultViewModel;

        public YourBankersContext Context { get; set; }

        private TabBaseViewModel _selectedTab;
        public TabBaseViewModel SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;

                if (value != null)
                { 
                    _selectedTab.TabVisibility = Visibility.Visible;
                    NotifyPropertyChanged("SelectedTab");
                }

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

            RegisterCommands();
            RegisterMessages();

            _loanApplicationSearchViewModel = new LoanApplicationSearchViewModel(TabMessenger, Context);
            _itemTabs.Add(_loanApplicationSearchViewModel);

            _clientSearchViewModel = new ClientSearchViewModel(TabMessenger, Context);
            _itemTabs.Add(_clientSearchViewModel);

            _clientDetailsViewModel = new ClientDetailsViewModel(TabMessenger, Context);
            _itemTabs.Add(_clientDetailsViewModel);

            _loanApplicationDetailsViewModel = new LoanApplicationDetailsViewModel(TabMessenger, Context);
            _itemTabs.Add(_loanApplicationDetailsViewModel);

            _summaryViewModel = new SummaryViewModel(TabMessenger, Context);
            _itemTabs.Add(_summaryViewModel);

            _resultViewModel = new ResultViewModel(TabMessenger, Context);
            _itemTabs.Add(_resultViewModel);
              
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
            OpenMainWindowSearchScreenCommand = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage { TabName = "Summary" });
            });
            OpenResultScreenCommand = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage { TabName = "Result" });
            });
        }

        public void RegisterMessages()
        {
            TabMessenger.Register<TabChangeMessage>(this,
                message =>
            {
                // TODO: Check why we cannot change from already fill up loan application to new loan application view.
                switch (message.TabName)
                {
                    case "ClientSearch":
                        SelectedTab = _clientSearchViewModel;
                        break;

                    case "ClientDetails":
                        _clientDetailsViewModel.SelectedClient = message.ObjectId > 0 ?
                           Context.Clients.Single(client => client.Id == message.ObjectId) : null;
                        SelectedTab = _clientDetailsViewModel;                       
                        break;

                    case "LoanApplicationSearch":
                        SelectedTab = _loanApplicationSearchViewModel;
                        break;

                    case "LoanApplicationDetails":
                        _loanApplicationDetailsViewModel.SelectedLoanApplication =
                            message.ObjectId > 0 ?
                            Context.LoanApplications.Single(loan => loan.Id == message.ObjectId) :
                            null;
                        SelectedTab = _loanApplicationDetailsViewModel;                                                                                                                                          
                        break;
                    case "Summary":
                        SelectedTab = _summaryViewModel;
                        break;
                    case "Result":
                        SelectedTab = _resultViewModel;
                        break;
                }
            });
        }
    }
}
