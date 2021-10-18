using CRMYourBankers.ViewModels.Interfaces;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace CRMYourBankers.ViewModels.Base
{
    //Base czyli klasa bazowa (tak jak obiektDoWYpozyczeń)
    //to jest, żeby przenosić tu części wspólne class z kategorii Tab czyli zakładek, czyli większości "Widoków"
    public class TabBaseViewModel : NotifyPropertyChangedBase, ITabMessengerOwner
    {
        public Messenger TabMessenger { get; set; }

        private Visibility _tabVisibility;
        public Visibility TabVisibility 
        {
            get => _tabVisibility; 
            set
            {
                _tabVisibility = value;

                if (_tabVisibility == Visibility.Visible)
                    RefreshData();
            }
        }

        public TabBaseViewModel() 
        {
            _tabVisibility = Visibility.Collapsed;
        }

        protected virtual void RefreshData() { }

        public TabBaseViewModel(Messenger messenger)
        {
            TabMessenger = messenger;
        }        
    }
}
