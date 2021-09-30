using GalaSoft.MvvmLight.Messaging;

namespace CRMYourBankers.ViewModels.Interfaces
{
    public interface ITabMessengerOwner
    //ITabMessengerOwner ta klasa powoduje, że możeby mieć kilka klas bazowych,
    //czyli decydujemy później, która klasa po której dziedziczy
    {
        Messenger TabMessenger { get; }
    }
}
