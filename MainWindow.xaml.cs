using AdminPartRM.ViewModels;
using ReadMangaApp.DataAccess;
using ReadMangaApp.View;
using System.Configuration;
using ReadMangaApp.Services;
using ReadMangaApp.Models;
using ReadMangaApp.Dtos;

namespace ReadMangaApp
{
    public partial class MainWindow
    {
        private readonly FrameNavigationService _navigationService;
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
            var dbConnection = new DBConnection(connectionString);
            _navigationService = new FrameNavigationService(MainContent);

            ConfigureNavigation(dbConnection);

            var vm = new MainWindowVM(_navigationService, dbConnection);
            DataContext = vm;

            vm.ToggleMenuRequested += (open) => MenuPopup.IsOpen = !MenuPopup.IsOpen;

            _navigationService.NavigateTo("MainMangaPage");
        }


        private void ConfigureNavigation(DBConnection dbConnection)
        {
            _navigationService.Configure("MainMangaPage", () => new MainMangaPage(_navigationService, dbConnection));
            _navigationService.Configure("ProfilePage", () => new ProfilePage(dbConnection));
            _navigationService.Configure("MangaDetailPage", param =>
            {
                if (param is Manga manga)
                {
                    return new MangaDetailPage(
                        manga,
                        manga.Genres,
                        manga.Tegs,
                        manga.MangaScores,
                    manga.Publishers,
                        dbConnection,
                        _navigationService
                    );
                }
                throw new ArgumentException("Invalid parameter for MangaDetailPage");
            });

        }
    }
}
