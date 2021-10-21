using CRMYourBankers.Database;
using CRMYourBankers.Messages;
using CRMYourBankers.Models;
using CRMYourBankers.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CRMYourBankers.ViewModels
{
    public class ResultViewModel : TabBaseViewModel
    {
        public ICommand DetailsScreenOpenHandler { get; set; }
        public YourBankersContext Context { get; set; }

        public ResultViewModel(Messenger messenger, YourBankersContext context) : base(messenger)
        {
            Context = context;
            RegisterCommands();
        }
        
        public void RegisterCommands()
        {
            DetailsScreenOpenHandler = new RelayCommand(() =>
            {
                TabMessenger.Send(new TabChangeMessage
                {
                    TabName = "Result"
                });
            });
        }

    }
}
