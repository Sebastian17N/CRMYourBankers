using GalaSoft.MvvmLight.Messaging;

namespace CRMYourBankers.ViewModels.Interfaces
{
    public interface ITabMessengerOwner
    {
        Messenger TabMessenger { get; }
    }
}
