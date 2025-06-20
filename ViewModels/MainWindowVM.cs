using BeautyShop.Commands;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.Services;
using System.Data;
using System.Data.Common;
using System.Windows.Input;

namespace ReadMangaApp.ViewModels
{
    internal class MainWindowVM : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly MangaApiClient _mangaApiClient; // Используем MangaApiClient вместо DBConnection
        private readonly AuthApiClient _authApiClient;
        public ICommand ToggleMenuCommand { get; }
        public ICommand OpenMangaWindowCommand { get; }
        public ICommand OpenProfileCommand { get; }
        public ICommand LoginOrLogoutCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand GoForwardCommand { get; }

        public string LoginButtonText => UserSession.Instance.CurrentUser != null ? "Выйти" : "Войти";

        public event Action<bool>? ToggleMenuRequested;

        public MainWindowVM(INavigationService navigationService, MangaApiClient mangaApiClient, AuthApiClient authApiClient)
        {
            _authApiClient = authApiClient;
            _navigationService = navigationService;
            _mangaApiClient = mangaApiClient;

            ToggleMenuCommand = new RelayCommand<object>(_ => ToggleMenu());
            OpenMangaWindowCommand = new RelayCommand<object>(_ => OpenMangaPage());
            OpenProfileCommand = new RelayCommand<object>(_ => OpenProfile());
            LoginOrLogoutCommand = new RelayCommand<object>(_ => LoginOrLogout());

            GoBackCommand = new RelayCommand<object>(_ => GoBack());
            GoForwardCommand = new RelayCommand<object>(_ => GoForward());

            UserSession.Instance.UserChanged += (s, e) => OnPropertyChanged(nameof(LoginButtonText));
        }

        private void GoBack()
        {
            _navigationService.GoBack();
        }

        private void GoForward()
        {
            _navigationService.GoForward();
        }

        private void ToggleMenu()
        {
            // Вместо прямого обращения к Popup — вызываем событие, чтобы View могла открыть/закрыть меню
            ToggleMenuRequested?.Invoke(true);
        }

        private void OpenMangaPage()
        {
            _navigationService.NavigateTo("MainMangaPage");
            ToggleMenu();
        }

        private void LoginOrLogout()
        {
            if (UserSession.Instance.CurrentUser == null)
            {
                AppServices.DialogService.ShowLoginDialog(_authApiClient);
            }
            else
            {
                UserSession.Instance.Logout();
                _navigationService.NavigateTo("MainMangaPage");
            }
        }

        private void OpenProfile()
        {
            if (UserSession.Instance.CurrentUser == null)
            {
                AppServices.DialogService.ShowMessage("Вы не авторизованы!", "Предупреждение");
            }
            else
            {
                _navigationService.NavigateTo("ProfilePage");
                ToggleMenu();
            }
        }
    }
}
