using AdminPartRM.ViewModels;
using ReadMangaApp.DataAccess;
using ReadMangaApp.View;
using System.Configuration;
using System.Windows.Controls.Primitives;
using System.Windows;
using ReadMangaApp.Services;
using ReadMangaApp.Models;

namespace ReadMangaApp
{
    public partial class MainWindow
    {
        private readonly FrameNavigationService _navigationService;
        private readonly DialogService _dialogService;
        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
            var dbConnection = new DBConnection(connectionString);
            _dialogService = new DialogService();
            _navigationService = new FrameNavigationService(MainContent);
            _navigationService.Configure("MainMangaPage", () => new MainMangaPage(_navigationService, _dialogService, dbConnection));
            _navigationService.Configure("ProfilePage", () => new ProfilePage());
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
                        dbConnection
                    );
                }
                throw new ArgumentException("Invalid parameter for MangaDetailPage");
            });

            var vm = new MainWindowVM(_navigationService, dbConnection, _dialogService);
            DataContext = vm;

            vm.ToggleMenuRequested += (open) => MenuPopup.IsOpen = !MenuPopup.IsOpen;

            _navigationService.NavigateTo("MainMangaPage");
        }
    }
}
