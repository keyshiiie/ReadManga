using ReadMangaApp.DataAccess;
using ReadMangaApp.Models;
using ReadMangaApp.ViewModels;
using System.Configuration;
using System.Windows.Controls;

namespace ReadMangaApp.View
{
    /// <summary>
    /// Логика взаимодействия для MangaInfoPage.xaml
    /// </summary>
    public partial class MangaInfoPage : Page
    {
        private MangaDetailPage _mangaDetailPage;
        public MangaInfoPage(MangaDetailPage mangaDetailPage, Manga selectedManga, IEnumerable<Genre> genres, IEnumerable<Teg> tegs)
        {
            InitializeComponent();
            _mangaDetailPage = mangaDetailPage;
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
            var dbConnection = new DBConnection(connectionString);
            DataContext = new MangaInfoPageVM(_mangaDetailPage, dbConnection, selectedManga, genres, tegs);
        }
    }
}
