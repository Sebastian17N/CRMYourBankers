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
        public Visibility TabVisibility { get; set; } = Visibility.Collapsed;

        public TabBaseViewModel() { }

        public TabBaseViewModel(Messenger messenger)
        {
            TabMessenger = messenger;
        }        
    }
}
