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
        public MangaInfoPage(Manga selectedManga, IEnumerable<Genre> genres, IEnumerable<Teg> tegs)
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;
            var dbConnection = new DBConnection(connectionString);
            DataContext = new MangaInfoPageVM(dbConnection, selectedManga, genres, tegs);
        }
    }
}
