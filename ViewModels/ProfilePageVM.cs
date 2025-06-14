using ReadMangaApp.Models;
using BeautyShop.Commands;
using System.Windows.Input;
using ReadMangaApp.DataAccess;

namespace ReadMangaApp.ViewModels
{
    public class ProfilePageVM : ViewModelBase
    {
        private string? _username;
        private string? _email;

        public string? Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        public string? Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        public ICommand EditProfileCommand { get; }

        public ProfilePageVM(DBConnection dBConnection)
        {
            var user = UserSession.Instance.CurrentUser;
            Username = user?.Username;
            Email = user?.Email;

            UserSession.Instance.UserChanged += OnUserChanged;
            EditProfileCommand = new RelayCommand<object>(_ => EditProfile());
        }

        private void OnUserChanged(object? sender, User? user)
        {
            Username = user?.Username;
            Email = user?.Email;
        }

        private void EditProfile()
        {
            // Логика редактирования профиля
        }
    }
}