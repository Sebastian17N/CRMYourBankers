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
        public string LabelText2
        {
            get => _labelText2;
            set
            {
                _labelText2 = value;
                NotifyPropertyChanged("LabelText2");
            }
        }
        public string LabelText3
        {
            get => _labelText3;
            set
            {
                _labelText3 = value;
                NotifyPropertyChanged("LabelText3");
            }
        }

        public string TextBoxText { get; set; }

        private string _labelText = "Wciśnij Przycisk i zobaczysz!";
        private string _labelText2 = "Nowe informacje";
        private string _labelText3 = "Nie uwierzysz co sie stanie!";

        public ICommand ChangeLabelButtonCommand { get; set; }
        public ICommand ChangeLabelButtonCommand2 { get; set; }
        public ICommand ChangeLabelButtonCommand3 { get; set; }
        public ICommand ChangeLabelButtonCommand4 { get; set; }

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
                LabelText2 = "";
            });
            ChangeLabelButtonCommand2 = new RelayCommand(() =>
            {               
                LabelText2 = TextBoxText;
                LabelText = "";
            });
            ChangeLabelButtonCommand3 = new RelayCommand(() =>
            {
                LabelText3 = TextBoxText;
            });
            ChangeLabelButtonCommand4 = new RelayCommand(() =>
            {              
                LabelText3 = "CheckBox zaznaczony";
            });            
        }        
    }
}
