using ReadMangaApp.DataAccess;
using ReadMangaApp.ViewModels;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ReadMangaApp.View
{
    /// <summary>
    /// Логика взаимодействия для MainMangaPage.xaml
    /// </summary>
    public partial class MainMangaPage : Page
    {
        private MainWindow _mainWindow;
        public MainMangaPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
            var dbConnection = new DBConnection(connectionString);

            // Передаем все необходимые параметры в MainMangaPageVM
            DataContext = new MainMangaPageVM(this, _mainWindow, dbConnection);
        }

        // много мороки с тем чтобы вынести это в VM проще всего реализовать это тут
        private void GenreToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var toggleButton = sender as ToggleButton;
            if (toggleButton != null)
            {
                GenreItemsControl.Visibility = toggleButton.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                toggleButton.Content = toggleButton.IsChecked == true ? "▲" : "▼"; // Изменяем текст кнопки
            }
        }

        private void TegToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var toggleButton = sender as ToggleButton;
            if (toggleButton != null)
            {
                TegItemsControl.Visibility = toggleButton.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                toggleButton.Content = toggleButton.IsChecked == true ? "▲" : "▼"; // Изменяем текст кнопки
            }
        }

        private void PublisherToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var toggleButton = sender as ToggleButton;
            if (toggleButton != null)
            {
                PublisherItemsControl.Visibility = toggleButton.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                toggleButton.Content = toggleButton.IsChecked == true ? "▲" : "▼"; // Изменяем текст кнопки
            }
        }
    }
}
