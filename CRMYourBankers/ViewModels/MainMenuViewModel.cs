using CRMYourBankers.Messages;
using CRMYourBankers.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;

namespace CRMYourBankers.ViewModels
{
    public class MainMenuViewModel : TabBaseViewModel
    {
        public ICommand OpenClientsSearchScreenCommand { get; set; }

        public MainMenuViewModel(Messenger messenger) : base(messenger)
        {
            RegisterCommands();
        }

        public void RegisterCommands()
        {
            OpenClientsSearchScreenCommand = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage { TabName = "ClientSearch" });
            });
        }
    }
}
