using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.Services;
using ReadMangaApp.ViewModels;
using System.Configuration;
using System.Data.Common;
using System.Windows.Controls;
using System.Windows.Input;

namespace ReadMangaApp.View
{
    /// <summary>
    /// Логика взаимодействия для ChaptersPage.xaml
    /// </summary>
    public partial class ChaptersPage : Page
    {
        public ChaptersPage(INavigationService mainNavigationService, IEnumerable<Chapter> chapters, DBConnection dBConnection)
        {
            InitializeComponent();
            var viewModel = new ChaptersPageVM(mainNavigationService, dBConnection, chapters);
            DataContext = viewModel;
        }

        private void ChapterTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock && textBlock.Tag is Chapter chapter)
            {
                var command = (DataContext as ChaptersPageVM)?.ReadPageChapterCommand;
                if (command != null && command.CanExecute(chapter))
                {
                    command.Execute(chapter);
                }
            }
        }
    }
}
