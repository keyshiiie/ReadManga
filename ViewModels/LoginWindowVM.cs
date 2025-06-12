using ReadMangaApp.Models;
using BeautyShop.Commands;
using ReadMangaApp.Commands;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Repository;
using ReadMangaApp.View;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ReadMangaApp.ViewModels
{
    internal class LoginWindowVM : INotifyPropertyChanged
    {
        private string _username = string.Empty;
        private string _password = string.Empty;
        public ICommand LoginCommand { get; }
        public ICommand OpenRegistrationWindowCommand { get; }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private bool _isAuthenticated;
        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                _isAuthenticated = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LoginButtonText));
            }
        }
        public string LoginButtonText => IsAuthenticated ? "Выход" : "Войти";


        private DBConnection _dbConnection;
        private LoginWindow _loginWindow;
        private User? user;

        public LoginWindowVM(DBConnection dbConnection, LoginWindow loginWindow) // Изменяем конструктор
        {
            _dbConnection = dbConnection;
            _loginWindow = loginWindow; // Сохраняем ссылку на окно
            LoginCommand = new RelayCommand<object>(_ => Login());
            OpenRegistrationWindowCommand = new RelayCommand<object>(_ => OpenRegistrationWindow());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Login()
        {
            // Получаем хеш пароля
            string hashedPassword = PasswordHasher.HashPassword(Password);

            // Получаем пользователя из репозитория
            var users = UserRepository.AuthorizationUser(_dbConnection, Username, hashedPassword);

            if (users.Any())
            {
                user = users.First(); // Сохраняем пользователя

                // Сохраняем в сессию
                UserSession.Instance.CurrentUser = user;
                _loginWindow.Close();
            }
            else
            {
                DisplayError("Неверный логин или пароль.");
            }
        }


        private void DisplayError(string message)
        {
            MessageBox.Show(message, "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void OpenRegistrationWindow()
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show(); // Открываем главное окно
            _loginWindow.Close();
        }
    }
}
