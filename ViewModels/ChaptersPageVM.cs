using AdminPartRM.ViewModels;
using BeautyShop.Commands;
using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.Repository;
using ReadMangaApp.View;
using System.Windows;
using System.Windows.Input;

namespace ReadMangaApp.ViewModels
{
    internal class ChaptersPageVM
    {
        public IEnumerable<Chapter> Chapters { get; }
        public ICommand ReadPageChapterCommand { get; }
        private DBConnection _dbConnection;
        public ChaptersPageVM(DBConnection dBConnection, IEnumerable<Chapter> chapters)
        {
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
                MessageBox.Show("В выбранной главе нет страниц.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Прерываем выполнение метода, если страницы отсутствуют
            }

            // Если страницы есть, передаем выбранную главу и список глав в ChapterReadPage
            var chapterReadPage = new ChapterReadPage(selectedChapter, Chapters.ToList());
            // Устанавливаем содержимое Frame в MainWindow
            // _mainWindow.MainContent.Navigate(chapterReadPage);
        }

        private List<MangaPage> GetPagesFromDatabase(int chapterId)
        {
            return PagesRepository.GetAllPages(_dbConnection, chapterId); // Получаем страницы по ID главы
        }
    }
}
