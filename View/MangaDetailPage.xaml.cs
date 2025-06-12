using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.ViewModels;
using System.Configuration;
using System.Windows.Controls;
using System.Windows;

namespace ReadMangaApp.View
{
    /// <summary>
    /// Логика взаимодействия для MangaDetailPage.xaml
    /// </summary>
    public partial class MangaDetailPage
    {
        private MainWindow _mainWindow;
        public MangaDetailPage(Manga selectedManga, List<Genre> genres, List<Teg> tegs, MangaScores? mangaScores, MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            // Извлечение строки подключения из конфигурации
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
            var dbConnection = new DBConnection(connectionString);
            DataContext = new MangaDetailPageVM(this, selectedManga, genres, tegs, mangaScores, _mainWindow, dbConnection);
            LoadInitialPage();
        }

        private void CollectionsComboBox_DropDownOpened(object sender, EventArgs e)
        {
            if (UserSession.Instance.CurrentUser == null)
            {
                MessageBox.Show("Вы не авторизованы! Пожалуйста, войдите в систему.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);

                // Закрываем выпадающий список, чтобы пользователь не видел пустой или недоступный список
                if (sender is ComboBox comboBox)
                {
                    comboBox.IsDropDownOpen = false;
                }
            }
        }



        private void LoadInitialPage()
        {
            if (DataContext is MangaDetailPageVM vm)
            {
                MangaDetailContent.Navigate(new MangaInfoPage(this, vm.SelectedManga, vm.Genres, vm.Tegs));
            }
        }
    }
}
