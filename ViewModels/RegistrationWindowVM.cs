using BeautyShop.Commands;
using ReadMangaApp.View;
using System.Windows.Input;
using ReadMangaApp.DataAccess;

namespace ReadMangaApp.ViewModels
{
    internal class RegistrationWindowVM : ViewModelBase
    {
        public ICommand OpenLoginWindowCommand { get; }

        private DBConnection _dbConnection;
        private RegistrationWindow _registrationWindow; // Добавляем поле для хранения ссылки на LoginWindow

        public RegistrationWindowVM(DBConnection dbConnection, RegistrationWindow registrationWindow) // Изменяем конструктор
        {
            _dbConnection = dbConnection;
            _registrationWindow = registrationWindow;
            OpenLoginWindowCommand = new RelayCommand<object>(_ => OpenLoginWindow());
        }

        public void OpenLoginWindow()
        {
            
        }
    }
}
