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
                {
                    if (this is IClearAllFieldsOwner)
                        ((IClearAllFieldsOwner)this).ClearAllFields();

                    if (this is IRefreshReferenceDataOwner)
                        ((IRefreshReferenceDataOwner)this).RefreshReferenceData();
                    // Sprawdź czy dany obiekt (widokmodel, np. LoadApplicationDetailsViewModel) implementuje interface
                    // IRefreshReferenceDataOwner, JEŚLI TAK to powiedz mu, żeby zachowywał się jak on i wywołał
                    // RefreshReferenceData, którego to deklaracja jest zawarta w tym interface.

                    if (this is IRefreshDataOwner)
                        ((IRefreshDataOwner)this).RefreshData();
                }
            }
        }

        public TabBaseViewModel() 
        {
            _tabVisibility = Visibility.Collapsed;
        }   

        public TabBaseViewModel(Messenger messenger)
        {
            TabMessenger = messenger;
        }        
    }
}
