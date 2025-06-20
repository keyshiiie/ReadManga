using ReadMangaApp.Models;
using BeautyShop.Commands;
using ReadMangaApp.Commands;
using ReadMangaApp.DataAccess;
using ReadMangaApp.View;
using System.Windows.Input;
using ReadMangaApp.Services;
using System.Threading.Tasks;
using System;

namespace ReadMangaApp.ViewModels
{
    internal class LoginWindowVM : ViewModelBase
    {
        private readonly AuthApiClient _authApiClient;
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

        private User? user;

        public LoginWindowVM(AuthApiClient authApiClient)
        {
            _authApiClient = authApiClient;
            LoginCommand = new RelayCommand<object>(async _ => await LoginAsync());
            OpenRegistrationWindowCommand = new RelayCommand<object>(_ => OpenRegistrationWindow());
        }

        private async Task LoginAsync()
        {
            try
            {
                // Запрашиваем авторизацию через API
                user = await _authApiClient.LoginAsync(Username, Password);

                if (user != null)
                {
                    UserSession.Instance.CurrentUser = user;
                    IsAuthenticated = true;
                    RequestClose?.Invoke();
                }
                else
                {
                    // Неверный логин/пароль
                    DisplayError("Неверный логин или пароль.");
                    IsAuthenticated = false;
                }
            }
            catch (Exception ex)
            {
                DisplayError($"Ошибка при попытке входа: {ex.Message}");
                IsAuthenticated = false;
            }
        }

        private void DisplayError(string message)
        {
            AppServices.DialogService.ShowMessage(message, "Ошибка авторизации");
        }

        public void OpenRegistrationWindow()
        {
            // Логика открытия окна регистрации
        }
    }
}
