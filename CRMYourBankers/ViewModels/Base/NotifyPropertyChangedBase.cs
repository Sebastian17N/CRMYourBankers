using System.ComponentModel;

namespace CRMYourBankers.ViewModels.Base
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //invoke wywołaj, viewModel powiadamia view, że zmieniła sie properta
        }
    }
}
