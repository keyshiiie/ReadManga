using ReadMangaApp.Dtos;
using ReadMangaApp.ViewModels;
using ReadMangaApp.DataAccess;  // для PageApiClient

namespace ReadMangaApp.View
{
    public partial class ChapterReadPage
    {
        private readonly PageApiClient _pageApiClient;

        public ChapterReadPage(ChapterReadPageParams param, PageApiClient pageApiClient)
        {
            InitializeComponent();

            _pageApiClient = pageApiClient;

            var viewModel = new ChapterReadPageVM(param.Chapter, param.Chapters, param.Pages, _pageApiClient);
            DataContext = viewModel;
        }
    }
}
