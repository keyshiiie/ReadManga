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
        public LoginWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
            var dbConnection = new DBConnection(connectionString);
            DataContext = new LoginWindowVM(dbConnection, this); // Передаем текущее окно
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
