using CRMYourBankers.Messages;
using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        //
        //TODO: 1. Dodać logikę edycji klienta. Czyli jeśli ekran jest otwarty z już istniejącym
        //         klientem, to walidacja peselu musi nie brać pod uwagę, wpisu który edytujesz (LINQ).
        //TODO: 2. Dodać nowy ekran do tworzenia/ edytowania wniosków, do którego można przejść
        //         z ekranu edycji klienta.
        // ObservableCollection co dokłądnie robi cała ta właściwość i druga tez?
        // dlaczego piszesz _costam?

        public ICommand OpenClientsSearchScreenCommand { get; set; }
        public ICommand AddNewClientButtonCommand { get; set; }
        public ICommand AddNewLoanApplicationCommand { get; set; }

        public ObservableCollection<object> ItemTabs => _itemTabs;

        public ObservableCollection<object> _itemTabs = new ObservableCollection<object>();

        public MainMenuViewModel _mainMenuViewModel;
        public ClientSearchViewModel _clientSearchViewModel;
        public ClientDetailsViewModel _clientDetailsViewModel;

        public List<Client> Clients { get; set; }

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
                    FirstName = "Piotr", 
                    LastName ="Zieliński", 
                    PhoneNumber = 888777999, 
                    Email = "zielinski@wp.pl", 
                    PersonalId = 12121212345
                },
                new Client 
                {
                    FirstName = "Jan", 
                    LastName ="Kowalski",
                    PhoneNumber = 555444666,
                    Email = "kowalski@onet.pl",
                    PersonalId = 55443312345
                }
            };

            RegisterCommands();
            RegisterMessages();

            _mainMenuViewModel = new MainMenuViewModel(TabMessenger);
            _itemTabs.Add(_mainMenuViewModel);

            _clientSearchViewModel = new ClientSearchViewModel(Clients, TabMessenger);
            _itemTabs.Add(_clientSearchViewModel);

            _clientDetailsViewModel = new ClientDetailsViewModel(TabMessenger, Clients);
            _itemTabs.Add(_clientDetailsViewModel);

            SelectedTab = _mainMenuViewModel;
        }

        protected void RegisterCommands()
        {
            AddNewClientButtonCommand = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage {TabName = "ClientDetails" });
            });
            AddNewLoanApplicationCommand = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage { TabName = "LoanApplicationDetails" });
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
                }
            });
        }
    }
}
