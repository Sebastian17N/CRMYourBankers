using CRMYourBankers.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace CRMYourBankers.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        public string LabelText
        {
            get => _labelText;
            set
            {
                _labelText = value;
                NotifyPropertyChanged("LabelText");
            }
        }
        public string TextBoxText { get; set; }

        private string _labelText = "Wciśnij Przycisk i zobaczysz!";

        public ICommand ChangeLabelButtonCommand { get; set; }

        public MainWindowViewModel()
        {
            RegisterCommands();
        }

        protected void RegisterCommands()
        {
            // Definicja funkcja anonimowej, która jest zasilona do tego co wykona się po wywołaniu 
            // danego Command.
            ChangeLabelButtonCommand = new RelayCommand(() =>
            {
                LabelText = TextBoxText;
            });
        }
        
    }
}
