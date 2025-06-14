using BeautyShop.Commands;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Dtos;
using ReadMangaApp.Models;
using ReadMangaApp.Repository;
using ReadMangaApp.Services;
using System.Windows.Input;

namespace ReadMangaApp.ViewModels
{
    internal class ChaptersPageVM : ViewModelBase
    {
        public Action<bool>? SetFullScreenContent { get; set; }


        private readonly INavigationService _navigationService;

        public IEnumerable<Chapter> Chapters { get; }
        public ICommand ReadPageChapterCommand { get; }
        private DBConnection _dbConnection;
        public ChaptersPageVM(INavigationService mainNavigationService, DBConnection dBConnection, IEnumerable<Chapter> chapters)
        {
            _navigationService = mainNavigationService;
            _dbConnection = dBConnection;
            ReadPageChapterCommand = new RelayCommand<Chapter>(chapter => ReadChapter(chapter));
            Chapters = chapters;
        }

        private void ReadChapter(Chapter selectedChapter)
        {
            // Получаем страницы для выбранной главы
            var pages = GetPagesFromDatabase(selectedChapter.Id);

            if (pages == null || !pages.Any())
            {
                // Если страницы отсутствуют, показываем предупреждение пользователю
                AppServices.DialogService.ShowMessage("В выбранной главе нет страниц.", "Ошибка");
                return; // Прерываем выполнение метода, если страницы отсутствуют
            }

            var param = new ChapterReadPageParams(
                Chapters.ToList(),
                selectedChapter      
            );
            _navigationService.NavigateTo("ChapterReadPage", param);
        }

        private List<MangaPage> GetPagesFromDatabase(int chapterId)
        {
            return PagesRepository.GetAllPages(_dbConnection, chapterId); // Получаем страницы по ID главы
        }
    }
}
