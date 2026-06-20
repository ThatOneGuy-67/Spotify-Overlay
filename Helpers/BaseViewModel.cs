using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SpotifyOverlay.Helpers
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected bool SetProperty<T>(ref T field, T value,
            [CallerMemberName] string? property = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(property);
            return true;
        }
    }
}