using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace View4Logs.UI.Base
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool Set<T>(ref T field, T newValue, [CallerMemberName]string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(newValue, field))
            {
                return false;
            }

            field = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected ObservableProperty<T> CreateProperty<T>(string propertyName)
        {
            return new ObservableProperty<T>(propertyName, RaisePropertyChanged);
        }

        protected ObservableProperty<T> CreateProperty<T>(string propertyName, T initialValue)
        {
            return new ObservableProperty<T>(propertyName, RaisePropertyChanged, initialValue);
        }
    }
}
