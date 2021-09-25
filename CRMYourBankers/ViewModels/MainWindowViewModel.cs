using CRMYourBankers.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace CRMYourBankers.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        //TODO: 1. Stwórz dwa przyciski, zmieniające tekst w dwóch różnych Label na widoku głównym.
        //TODO: 2. Jeśli jeden text pojawi się w Label1, to Label2 powienien być wyczyszczony i odwrotnie.
        //TODO: 3. Dodaj CheckBox, który będzie wyświetlał w Label3, informację tekstową, czy jest zaznaczony.

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
