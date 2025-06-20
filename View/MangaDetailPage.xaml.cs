using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.ViewModels;
using System.Windows.Controls;
using System.Windows;
using ReadMangaApp.Services;
using ReadMangaApp.Dtos;
using System.Data.Common;
using System.Data;

namespace ReadMangaApp.View
{
    /// <summary>
    /// Логика взаимодействия для MangaDetailPage.xaml
    /// </summary>
    public partial class MangaDetailPage
    {
        private readonly PageApiClient _pageApiClient;
        private readonly FrameNavigationService _mainNavigationService;     // основной фрейм
        private readonly FrameNavigationService _localNavigationService;    // вложенный фрейм
        public MangaDetailPage(Manga selectedManga, FrameNavigationService mainNavigationService, MangaScoreApiClient mangaScoreApiClient, MangaCollectionApiClient mangaCollectionApiClient, ChapterApiClient chapterApiClient, PageApiClient pageApiClient)
        {
            _pageApiClient = pageApiClient;
            InitializeComponent();
            _mainNavigationService = mainNavigationService;
            _localNavigationService = new FrameNavigationService(MangaDetailContent); // ← создаём локальный
            DataContext = new MangaDetailPageVM(_localNavigationService, selectedManga, mangaScoreApiClient, mangaCollectionApiClient, chapterApiClient, pageApiClient);

            ConfigureNavigation();

            _localNavigationService.NavigateTo("MangaInfoPage", selectedManga);
        }

        private void ConfigureNavigation()
        {
            _localNavigationService.Configure("ChaptersPage", param =>
            {
                if (param is ChaptersPageParams p)
                    return new MangaChaptersPage(_mainNavigationService, p.Chapters, p.PageApiClient);
                throw new ArgumentException("Неверные параметры для ChaptersPage");
            });

            _localNavigationService.Configure("MangaInfoPage", param =>
            {
                if (param is Manga selectedManga)
                    return new MangaInfoPage(_mainNavigationService, selectedManga);
                throw new ArgumentException("Неверные параметры для MangaInfoPage");
            });

            _mainNavigationService.Configure("ChapterReadPage", param =>
            {
                if (param is ChapterReadPageParams p)
                    return new ChapterReadPage(p, _pageApiClient);  // Передаём pageApiClient в конструктор
                throw new ArgumentException("Invalid parameter for ChapterReadPage");
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