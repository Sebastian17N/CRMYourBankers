using CRMYourBankers.Messages;
using CRMYourBankers.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;

namespace CRMYourBankers.ViewModels
{
    public class LoanApplicationSearchViewModel : TabBaseViewModel
    {     
        public LoanApplicationSearchViewModel(Messenger messenger) : base(messenger)
        {
            RegisterCommands();
        }

        public void RegisterCommands()
        {
           
        }
    }
}
