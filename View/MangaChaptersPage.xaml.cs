using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.Services;
using ReadMangaApp.ViewModels;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Windows.Controls;
using System.Windows.Input;

namespace ReadMangaApp.View
{
    /// <summary>
    /// Логика взаимодействия для ChaptersPage.xaml
    /// </summary>
    public partial class MangaChaptersPage : Page
    {
        public MangaChaptersPage(INavigationService mainNavigationService, IEnumerable<Chapter> chapters, PageApiClient pageApiClient)
        {
            InitializeComponent();
            var viewModel = new MangaChaptersPageVM(mainNavigationService, chapters, pageApiClient);
            DataContext = viewModel;
        }

        private void ChapterTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock && textBlock.Tag is Chapter chapter)
            {
                var command = (DataContext as MangaChaptersPageVM)?.ReadPageChapterCommand;
                if (command != null && command.CanExecute(chapter))
                {
                    command.Execute(chapter);
                }
            }
        }
    }
}