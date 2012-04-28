using System.ComponentModel;

namespace BubbleChart.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if(handler != null) handler(this, e);
        }

        public void OnPropertyChanged(string propName)
        {
            var args = new PropertyChangedEventArgs(propName);
            OnPropertyChanged(args);
        }

        public void RaisePropertyChanged(string propName)
        {
            OnPropertyChanged(propName);
        }

        protected bool SetValue<T>(ref T backField, T newValue, string propName)
        {
            if (Equals(backField,newValue)) return false;
            backField = newValue;
            RaisePropertyChanged(propName);
            return true;
        }
    }
}