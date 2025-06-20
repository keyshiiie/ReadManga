using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.Services;
using ReadMangaApp.ViewModels;
using System;
using System.Data.Common;
using System.Net.Http;

namespace ReadMangaApp.View
{
    public partial class MainWindow
    {
        private readonly FrameNavigationService _navigationService;

        public MainWindow()
        {
            InitializeComponent();

            var httpClient = new HttpClient { BaseAddress = new Uri("https://readmangaserver.onrender.com/api/") };

            // Создаем один контейнер с клиентами API
            var apiClients = new ApiClientsBundle(httpClient);

            _navigationService = new FrameNavigationService(MainContent);

            // Передаем контейнер в конфигурацию навигации
            ConfigureNavigation(apiClients);

            var vm = new MainWindowVM(_navigationService, apiClients.MangaApiClient, apiClients.AuthApiClient);
            DataContext = vm;

            vm.ToggleMenuRequested += (open) => MenuPopup.IsOpen = !MenuPopup.IsOpen;

            _navigationService.NavigateTo("MainMangaPage");
        }

        private void ConfigureNavigation(ApiClientsBundle apiClients)
        {
            // Передаем контейнер в конструктор страницы
            _navigationService.Configure("MainMangaPage", () => new MainMangaPage(
                _navigationService,
                apiClients));
            _navigationService.Configure("MangaDetailPage", param =>
            {
                if (param is Manga manga)
                {
                    return new MangaDetailPage(manga, _navigationService, apiClients.MangaScoreApiClient, apiClients.MangaCollectionApiClient, apiClients.ChapterApiClient, apiClients.PageApiClient);
                }
                throw new ArgumentException("Invalid parameter for MangaDetailPage");
            });
        }
    }
}
