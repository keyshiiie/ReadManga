using BeautyShop.Commands;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.Services;
using System.Windows.Input;

namespace AdminPartRM.ViewModels
{
    internal class MainWindowVM : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly DBConnection _dbConnection;

        public ICommand ToggleMenuCommand { get; }
        public ICommand OpenMangaWindowCommand { get; }
        public ICommand LoginOrLogoutCommand { get; }
        public ICommand OpenProfileCommand { get; }

        public string LoginButtonText => UserSession.Instance.CurrentUser != null ? "Выйти" : "Войти";

        public event Action<bool>? ToggleMenuRequested;

        private readonly IDialogService _dialogService;

        public MainWindowVM(INavigationService navigationService, DBConnection dbConnection, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dbConnection = dbConnection;
            _dialogService = dialogService;

            ToggleMenuCommand = new RelayCommand<object>(_ => ToggleMenu());
            OpenMangaWindowCommand = new RelayCommand<object>(_ => OpenMangaPage());
            LoginOrLogoutCommand = new RelayCommand<object>(_ => LoginOrLogout());
            OpenProfileCommand = new RelayCommand<object>(_ => OpenProfile());

            UserSession.Instance.UserChanged += (s, e) => OnPropertyChanged(nameof(LoginButtonText));
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
                _dialogService.ShowLoginDialog();
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
                _dialogService.ShowMessage("Вы не авторизованы!", "Предупреждение");
            }
            else
            {
                _navigationService.NavigateTo("ProfilePage");
                ToggleMenu();
            }
        }
    }
}
