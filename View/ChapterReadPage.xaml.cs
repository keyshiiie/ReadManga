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
    /// Логика взаимодействия для ChapterReadPage.xaml
    /// </summary>
    public partial class ChapterReadPage
    {
        public ChapterReadPage(Chapter selectedChapter, List<Chapter> chapters)
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
            var dbConnection = new DBConnection(connectionString);
            DataContext = new ChapterReadPageVM(selectedChapter, chapters, dbConnection);
        }
    }
}
