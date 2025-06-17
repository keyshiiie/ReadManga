using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.ViewModels;
using System.Windows.Controls;
using System.Windows;
using ReadMangaApp.Services;
using ReadMangaApp.Dtos;

namespace ReadMangaApp.View
{
    /// <summary>
    /// Логика взаимодействия для MangaDetailPage.xaml
    /// </summary>
    public partial class MangaDetailPage
    {
        private readonly FrameNavigationService _mainNavigationService;     // основной фрейм
        private readonly FrameNavigationService _localNavigationService;    // вложенный фрейм
        public MangaDetailPage(Manga selectedManga, DBConnection dbConnection, FrameNavigationService mainNavigationService)
        {
            InitializeComponent();
            _mainNavigationService = mainNavigationService;
            _localNavigationService = new FrameNavigationService(MangaDetailContent); // ← создаём локальный
            DataContext = new MangaDetailPageVM(_localNavigationService, selectedManga, dbConnection);

            ConfigureNavigation(dbConnection);

            _localNavigationService.NavigateTo("MangaInfoPage", selectedManga);
        }

        private void ConfigureNavigation(DBConnection dbConnection)
        {
            _localNavigationService.Configure("ChaptersPage", param =>
            {
                if (param is ChaptersPageParams p)
                    return new ChaptersPage(_mainNavigationService, p.Chapters, dbConnection); // ← передаём основной сервис
                throw new ArgumentException("Неверные параметры для ChaptersPage");
            });

            _localNavigationService.Configure("MangaInfoPage", param =>
            {
                if (param is Manga selectedManga)
                    return new MangaInfoPage(_mainNavigationService, selectedManga, dbConnection);
                throw new ArgumentException("Неверные параметры для MangaInfoPage");
            });

            // Важно: ChapterReadPage открывается в основном фрейме
            _mainNavigationService.Configure("ChapterReadPage", param =>
            {
                if (param is ChapterReadPageParams p)
                    return new ChapterReadPage(p.chapter, p.Chapters, dbConnection);
                throw new ArgumentException("Invalid parameter for ChapterReadPage");
            });

            // MainMangaPage

            _mainNavigationService.Configure("MainMangaPage", param =>
            {
                if (param is Genre selectedGenre)
                {
                    return new MainMangaPage(_mainNavigationService, dbConnection, selectedGenre, null);
                }
                else if (param is Teg selectedTeg)
                {
                    return new MainMangaPage(_mainNavigationService, dbConnection, null, selectedTeg);
                }
                else if (param is null)
                {
                    return new MainMangaPage(_mainNavigationService, dbConnection, null, null);
                }
                throw new ArgumentException("Invalid parameter for MainMangaPage");
            });
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
    }
}
