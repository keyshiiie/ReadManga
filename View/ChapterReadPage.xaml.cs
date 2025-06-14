using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.ViewModels;

namespace ReadMangaApp.View
{
    /// <summary>
    /// Логика взаимодействия для ChapterReadPage.xaml
    /// </summary>
    public partial class ChapterReadPage
    {
        public ChapterReadPage(Chapter selectedChapter, List<Chapter> chapters, DBConnection dbConnection)
        {
            InitializeComponent();
            var viewModel = new ChapterReadPageVM(selectedChapter, chapters, dbConnection);
            DataContext = viewModel;
        }
    }
}
