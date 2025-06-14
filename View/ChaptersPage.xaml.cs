using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReadMangaApp.View
{
    /// <summary>
    /// Логика взаимодействия для ChaptersPage.xaml
    /// </summary>
    public partial class ChaptersPage : Page
    {
        public ChaptersPage(IEnumerable<Chapter> chapters)
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
            var dbConnection = new DBConnection(connectionString);
            DataContext = new ChaptersPageVM(dbConnection, chapters);
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
