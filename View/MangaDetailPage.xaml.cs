using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.ViewModels;
using System.Configuration;
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
        private readonly FrameNavigationService _navigationService;
        private readonly DialogService _dialogService;
        public MangaDetailPage(Manga selectedManga, List<Genre> genres, List<Teg> tegs, MangaScores? mangaScores, List<Publisher> publishers, DBConnection dbConnection)
        {
            InitializeComponent();
            LoadInitialPage();

            _dialogService = new DialogService();
            _navigationService = new FrameNavigationService(MangaDetailContent);
            DataContext = new MangaDetailPageVM(_navigationService, selectedManga, genres, tegs, mangaScores, publishers, dbConnection);

            _navigationService.Configure("MangaInfoPage", param =>
            {
                if (param is MangaInfoPageParams p)
                {
                    return new MangaInfoPage(p.Manga, p.Genres, p.Tegs);
                }
                throw new ArgumentException("Invalid parameters for MangaInfoPage");
            });

            _navigationService.NavigateTo("MangaInfoPage", new MangaInfoPageParams(
                selectedManga,
                genres,
                tegs
            ));
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
                MangaDetailContent.Navigate(new MangaInfoPage(vm.SelectedManga, vm.Genres, vm.Tegs));
            }
        }
    }
}
