using BeautyShop.Commands;
using ReadMangaApp;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.View;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace AdminPartRM.ViewModels
{
    internal class MainWindowVM : INotifyPropertyChanged
    {
        private readonly MainWindow _mainWindow;
        private readonly DBConnection _dbConnection;
        private LoginWindow? _loginWindow;

        public ICommand ToggleMenuCommand { get; }
        
        public ICommand OpenMangaWindowCommand { get; }
        public ICommand LoginOrLogoutCommand { get; }
        public ICommand OpenProfileCommand { get; }
        public string LoginButtonText => UserSession.Instance.CurrentUser != null ? "Выйти" : "Войти";

        public MainWindowVM(MainWindow mainWindow, DBConnection dbConnection)
        {
            _mainWindow = mainWindow;
            _dbConnection = dbConnection;
            ToggleMenuCommand = new RelayCommand<object>(_ => ToggleMenu());
            OpenMangaWindowCommand = new RelayCommand<object>(_ => OpenMangaPage());
            LoginOrLogoutCommand = new RelayCommand<object>(_ => LoginOrLogout());
            OpenProfileCommand = new RelayCommand<object>(_ => OpenProfile()); 
            UserSession.Instance.UserChanged += (s, e) => OnPropertyChanged(nameof(LoginButtonText));
        }

        private void ToggleMenu()
        {
            // Переключаем состояние Popup меню
            _mainWindow.MenuPopup.IsOpen = !_mainWindow.MenuPopup.IsOpen;
        }

        private void OpenMangaPage()
        {
            var mangaPage = new MainMangaPage(_mainWindow);
            _mainWindow.MainContent.Content = mangaPage;
            ToggleMenu();
        }

        private void LoginOrLogout()
        {
            if (UserSession.Instance.CurrentUser == null)
            {
                // Открываем окно авторизации
                if (_loginWindow == null)
                {
                    _loginWindow = new LoginWindow();
                    _loginWindow.Closed += (s, e) => _loginWindow = null;
                    _loginWindow.ShowDialog();
                }
            }
            else
            {
                // Выход из аккаунта
                UserSession.Instance.Logout();

                // Если открыта страница профиля — закрываем её
                if (_mainWindow.MainContent.Content is ProfilePage)
                {
                    _mainWindow.MainContent.Content = new MainMangaPage(_mainWindow);
                    // или, например, _mainWindow.MainContent.Content = new HomePage();
                }
            }
        }


        private void OpenProfile()
        {
            if (UserSession.Instance.CurrentUser == null)
            {
                MessageBox.Show("Вы не авторизованы!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var profilePage = new ProfilePage(_mainWindow);
                _mainWindow.MainContent.Content = profilePage;
                ToggleMenu();
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
