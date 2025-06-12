using BeautyShop.Commands;
using ReadMangaApp.Commands;
using ReadMangaApp.Repository;
using ReadMangaApp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using ReadMangaApp.DataAccess;

namespace ReadMangaApp.ViewModels
{
    internal class RegistrationWindowVM : INotifyPropertyChanged
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
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show(); // Открываем главное окно
            _registrationWindow.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
