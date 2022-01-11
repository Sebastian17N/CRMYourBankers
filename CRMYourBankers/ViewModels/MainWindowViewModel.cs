using CRMYourBankers.Database;
using CRMYourBankers.Messages;
using CRMYourBankers.ViewModels.Base;
using CRMYourBankers.ViewModels.Interfaces;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CRMYourBankers.Enums;
using CRMYourBankers.Models;
using System.IO;
using System;
using CRMYourBankers.Models.Interfaces;
using System.Collections.Generic;

namespace CRMYourBankers.ViewModels
{
    public class MainWindowViewModel : TabBaseViewModel
    {  
        public ICommand OpenClientsSearchScreenCommand { get; set; }
        public ICommand OpenLoanApplicationsSearchScreenCommand { get; set; }
        public ICommand AddNewClientButtonCommand { get; set; }
        public ICommand AddNewLoanApplicationCommand { get; set; }
        public ICommand OpenMainWindowSearchScreenCommand { get; set; }
        public ICommand OpenResultScreenCommand { get; set; }

        public List<(TabName tabName, IEditable selectedItem)> ScreensPath { get; set; } = new List<(TabName tabName, IEditable selectedItem)>();

        public ObservableCollection<object> ItemTabs => _itemTabs;
        private ObservableCollection<object> _itemTabs = new ObservableCollection<object>();
        //można wymiennie pisać z NotifyPropertyChanged

        private LoanApplicationSearchViewModel _loanApplicationSearchViewModel;
        private ClientSearchViewModel _clientSearchViewModel;
        private ClientDetailsViewModel _clientDetailsViewModel;
        private LoanApplicationDetailsViewModel _loanApplicationDetailsViewModel;
        private SummaryViewModel _summaryViewModel;
        private ResultViewModel _resultViewModel;

        public string LogoPath => $"{AppDomain.CurrentDomain.BaseDirectory}Images\\Logo.png";

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
            //Context.TestSeeds();

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
                TabMessenger.Send(new TabChangeMessage 
                { 
                    TabName = TabName.ClientDetails
                });
            });
            AddNewLoanApplicationCommand = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage
                { 
                    TabName = TabName.LoanApplicationDetails
                });
            });
            OpenClientsSearchScreenCommand = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage { TabName = TabName.ClientSearch });
            });
            OpenLoanApplicationsSearchScreenCommand = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage { TabName = TabName.LoanApplicationSearch });
            });
            OpenMainWindowSearchScreenCommand = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage { TabName = TabName.Summary });
            });
            OpenResultScreenCommand = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage { TabName = TabName.Result });
            });
        }

        public void RegisterMessages()
        {
            TabMessenger.Register<TabChangeMessage>(this,
                message =>
            {
                ActivatedClientStatusBaseOnIncomingTask();

                // 1. Wyciągać lastTabObject z ostatniej otwartej zakładki (o ile istnieje) - czyli np. przechodząc z client details -> dodaj nowy wniosek
                //    do LastTabObject trafi konkretny klient z poprzedniego widoku.
                // 1.b Dodać interface, który będzie zawierał definicję obiektu, po którym będą dziedziczyły LoanApplication i Client, i który będzie miał
                //    uniwersalne pole typu SelectedItem.
                // 2. Przypisujesz to LastTabObject poniżej w switchu.
                // 2.a Dodajesz logikę przepisania SelectedObject = LastTabObject jeśli jest to cofnięcie.
                // 3. Kasujesz przypisywanie LastTabObject ze wszystkich innych miejsc w kodzie.
                // 4. Jak rozpoznawać, że się cofasz, a nie zagnieżdżasz, może pole w SwitchTabMessage.

                var goFurther = message.GoFurther;
                IEditable tabObject = null;
                var tabName = TabName.Summary;

                if (goFurther)
                {// Zagnieżdżanie.
                    if (SelectedTab is ISelectedItemOwner<LoanApplication>)
                    {
                        tabObject = ((ISelectedItemOwner<LoanApplication>)SelectedTab).SelectedItem;
                    }
                    if (SelectedTab is ISelectedItemOwner<Client>)
					{
                        tabObject = ((ISelectedItemOwner<Client>)SelectedTab).SelectedItem;
					}
                    tabName = SelectedTab.TabName;

                    ScreensPath.Add((tabName, tabObject));
                }
                else
                {// Cofanie.
                    var lastScreen = ScreensPath.Last();
                    tabObject = lastScreen.selectedItem;
                    tabName = lastScreen.tabName;
                    ScreensPath.Remove(lastScreen);
                }

                var tabNameToGo = goFurther ? message.TabName : tabName;

                switch (tabNameToGo)
                {
                    case TabName.ClientSearch:
                        SelectedTab = _clientSearchViewModel;
                        break;

                    case TabName.ClientDetails:
                        _clientDetailsViewModel.SelectedItem = (Client)(goFurther ? message.SelectedObject : tabObject);
                        SelectedTab = _clientDetailsViewModel;                       
                        break;

                    case TabName.LoanApplicationSearch:
                        SelectedTab = _loanApplicationSearchViewModel;
                        break;

                    case TabName.LoanApplicationDetails:
                        _loanApplicationDetailsViewModel.SelectedItem = (LoanApplication)(goFurther ? message.SelectedObject : tabObject);
                        SelectedTab = _loanApplicationDetailsViewModel;
                            break;

                    case TabName.Summary:
                        SelectedTab = _summaryViewModel;
                        break;

                    case TabName.Result:
                        SelectedTab = _resultViewModel;
                        break;
                }
                               
            });
        }

        public void ActivatedClientStatusBaseOnIncomingTask()
        {
            //Kroki postępowania w pisaniu Linq:
            //znajdz nieaktywnych klientów
            //data zadania jest dzisiejsza, lub wsteczna oraz NotDone
            //zmiana statusu na aktywny

            var foundClients = Context.Clients
                .Where(client => client.ClientStatus != ClientStatus.Active)
                .Where(client => client.ClientTasks
                        .Any(task => task.TaskDate <= System.DateTime.Now && !task.Done))
                .ToList();
            foreach (var client in foundClients)
            {
                client.ClientStatus = ClientStatus.Active;
            }

            Context.SaveChanges();            
        }
    }
}
