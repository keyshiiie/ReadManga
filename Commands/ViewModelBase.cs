using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BeautyShop.Commands
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        // Явная реализация интерфейса
        event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
        {
            add { PropertyChanged += value; }
            remove { PropertyChanged -= value; }
        }

        // Защищенное событие
        protected event PropertyChangedEventHandler? PropertyChanged;

        // Защищенный виртуальный метод для уведомления об изменениях
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
