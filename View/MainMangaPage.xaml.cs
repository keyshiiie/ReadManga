using ReadMangaApp.DataAccess;
using ReadMangaApp.Services;
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
        private readonly INavigationService _navigationService;
        private readonly DBConnection _dbConnection;
        public MainMangaPage(INavigationService navigationService, DBConnection dbConnection)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _dbConnection = dbConnection;
            DataContext = new MainMangaPageVM(_navigationService, this, _dbConnection);
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
