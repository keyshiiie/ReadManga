using ReadMangaApp.DataAccess;
using ReadMangaApp.ViewModels;
using System.Configuration;
using System.Windows.Controls;
using System.Windows;

namespace ReadMangaApp.View
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow 
    {
        public LoginWindow(DBConnection dbConnection)
        {
            InitializeComponent();
            var viewModel = new LoginWindowVM(dbConnection);
            viewModel.RequestClose += () => this.Close();
            DataContext = viewModel;
        }


        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox) // Используем паттерн-матчинг
            {
                var vm = DataContext as LoginWindowVM;
                if (vm != null)
                {
                    vm.Password = passwordBox.Password;
                }
            }
        }
    }
}
