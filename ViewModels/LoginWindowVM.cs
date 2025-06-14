using ReadMangaApp.Models;
using BeautyShop.Commands;
using ReadMangaApp.Commands;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Repository;
using ReadMangaApp.View;
using System.Windows.Input;
using ReadMangaApp.Services;

namespace ReadMangaApp.ViewModels
{
    internal class LoginWindowVM : ViewModelBase
    {
        public event Action? RequestClose;
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
        private User? user;

        public LoginWindowVM(DBConnection dbConnection) // Изменяем конструктор
        {
            _dbConnection = dbConnection;
            LoginCommand = new RelayCommand<object>(_ => Login());
            OpenRegistrationWindowCommand = new RelayCommand<object>(_ => OpenRegistrationWindow());
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
                RequestClose?.Invoke();
            }
            else
            {
                DisplayError("Неверный логин или пароль.");
            }
        }

        private void DisplayError(string message)
        {
            AppServices.DialogService.ShowMessage(message, "Ошибка авторизации");
        }

        public void OpenRegistrationWindow()
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
        }
    }
}
