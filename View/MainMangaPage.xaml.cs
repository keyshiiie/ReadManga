using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.Services;
using ReadMangaApp.ViewModels;
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
        public MainMangaPage(INavigationService navigationService, DBConnection dbConnection, Genre? selectedGenre, Teg? selectedTeg)
        {
            InitializeComponent();
            _navigationService = navigationService;
            DataContext = new MainMangaPageVM(_navigationService, dbConnection, selectedGenre, selectedTeg);
        }

        // целесообразно обрабатывать нажатие кнопок здесь тк viewmodel не должна знать о view и управлять им
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
