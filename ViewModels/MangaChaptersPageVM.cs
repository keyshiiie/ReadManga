using BeautyShop.Commands;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Dtos;
using ReadMangaApp.Models;
using ReadMangaApp.Services;
using System.Data.Common;
using System.Net.Http;
using System.Windows.Input;

namespace ReadMangaApp.ViewModels
{
    internal class MangaChaptersPageVM : ViewModelBase
    {
        private readonly PageApiClient _pageApiClient;
        public Action<bool>? SetFullScreenContent { get; set; }


        private readonly INavigationService _navigationService;

        public IEnumerable<Chapter> Chapters { get; }
        public ICommand ReadPageChapterCommand { get; }
        public MangaChaptersPageVM(INavigationService mainNavigationService, IEnumerable<Chapter> chapters, PageApiClient pageApiClient)
        {
            _pageApiClient = pageApiClient;
            _navigationService = mainNavigationService;
            ReadPageChapterCommand = new RelayCommand<Chapter>(chapter => ReadChapter(chapter));
            Chapters = chapters;
        }

        private async void ReadChapter(Chapter selectedChapter)
        {
            try
            {
                var pages = await _pageApiClient.GetAllChapterPagesAsync(selectedChapter.Id);

                if (pages == null || !pages.Any())
                {
                    AppServices.DialogService.ShowMessage("В выбранной главе нет страниц.", "Информация");
                    return;
                }

                var param = new ChapterReadPageParams
                (
                    Chapters.ToList(),
                    selectedChapter,
                    pages.ToList()
                );
                _navigationService.NavigateTo("ChapterReadPage", param);
            }
            catch (Exception ex)
            {
                // Ловим все исключения, чтобы не падало приложение
                Console.WriteLine($"Ошибка при загрузке страниц главы: {ex.Message}");
                AppServices.DialogService.ShowMessage("Произошла ошибка при загрузке страниц главы.", "Ошибка");
            }
        }


    }
}